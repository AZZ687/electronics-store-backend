using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;

    public OrderService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponseDto> CreateAsync(CreateOrderDto orderDto,
    int userId)
    {
        var order = new Order
        {
            UserId = userId
        };

        foreach (var itemDto in orderDto.Items)
        {
            var product = await _context.Products
                .FindAsync(itemDto.ProductId);

            if (product == null)
            {
                throw new InvalidOperationException(
                    $"Product with ID {itemDto.ProductId} was not found.");
            }

            if (product.StockQuantity < itemDto.Quantity)
            {
                throw new InvalidOperationException(
                    $"Not enough stock for product '{product.Name}'.");
            }

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            };

            order.OrderItems.Add(orderItem);

            product.StockQuantity -= itemDto.Quantity;
        }

        order.TotalAmount = order.OrderItems
            .Sum(item => item.UnitPrice * item.Quantity);

        _context.Orders.Add(order);

        await _context.SaveChangesAsync();

        return new OrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Items = order.OrderItems.Select(item => new OrderItemResponseDto
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                SubTotal = item.UnitPrice * item.Quantity
            }).ToList()
        };
    }

    public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(item => item.Product)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(order => new OrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Items = order.OrderItems.Select(item => new OrderItemResponseDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                ImageUrl = item.Product.ImageUrl,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                SubTotal = item.UnitPrice * item.Quantity
            }).ToList()
        });
    }
    public async Task<IEnumerable<OrderResponseDto>> GetMyOrdersAsync(int userId)
    {
        var orders = await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ToListAsync();

        return orders.Select(o => new OrderResponseDto
        {
            Id = o.Id,
            UserId = o.UserId,
            CreatedAt = o.CreatedAt,
            TotalAmount = o.TotalAmount,
            Status = o.Status,
            Items = o.OrderItems.Select(item => new OrderItemResponseDto
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                SubTotal = item.UnitPrice * item.Quantity
            }).ToList()
        });
    }

    public async Task<OrderResponseDto?> GetByIdAsync(int orderId, int userId)
    {
        var order = await _context.Orders
    .Where(o => o.Id == orderId && o.UserId == userId)
    .Include(o => o.OrderItems)
        .ThenInclude(item => item.Product)
    .FirstOrDefaultAsync();

        if (order == null)
        {
            return null;
        }

        return new OrderResponseDto
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            Items = order.OrderItems.Select(item => new OrderItemResponseDto
            {
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                ImageUrl = item.Product.ImageUrl,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                SubTotal = item.UnitPrice * item.Quantity
            }).ToList()
        };
    }

    public async Task<bool> UpdateStatusAsync(int orderId, string status)
    {
        var order = await _context.Orders.FindAsync(orderId);

        if (order == null)
        {
            return false;
        }

        var allowedStatuses = new[]
        {
        "Pending",
        "Processing",
        "Shipped",
        "Delivered",
        "Cancelled"
    };

        if (!allowedStatuses.Contains(status))
        {
            return false;
        }

        order.Status = status;

        await _context.SaveChangesAsync();

        return true;
    }
}
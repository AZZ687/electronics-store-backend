using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTOs;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderDto orderDto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        var userId = int.Parse(userIdClaim.Value);

        try
        {
            var order = await _orderService.CreateAsync(
                orderDto,
                userId);

            return Ok(order);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new
            {
                message = ex.Message
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetMyOrders()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var orders = await _orderService.GetMyOrdersAsync(userId);

        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        if (!int.TryParse(userIdClaim.Value, out var userId))
        {
            return Unauthorized();
        }

        var order = await _orderService.GetByIdAsync(id, userId);

        if (order == null)
        {
            return NotFound();
        }

        return Ok(order);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _orderService.GetAllAsync();

        return Ok(orders);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(
    int id,
    UpdateOrderStatusDto dto)
    {
        var updated = await _orderService.UpdateStatusAsync(id, dto.Status);

        if (!updated)
        {
            return BadRequest("Invalid order ID or status.");
        }

        return Ok(new
        {
            message = "Order status updated successfully."
        });
    }
}
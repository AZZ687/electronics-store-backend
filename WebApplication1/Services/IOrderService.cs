using WebApplication1.DTOs;


namespace WebApplication1.Services;

public interface IOrderService
{
    Task<OrderResponseDto> CreateAsync(CreateOrderDto orderDto, int userId);
    Task<IEnumerable<OrderResponseDto>> GetMyOrdersAsync(int userId);
    Task<OrderResponseDto?> GetByIdAsync(int orderId, int userId);

    Task<IEnumerable<OrderResponseDto>> GetAllAsync();

    Task<bool> UpdateStatusAsync(int orderId, string status);
}
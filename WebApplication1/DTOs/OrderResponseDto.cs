namespace WebApplication1.DTOs;

public class OrderResponseDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = string.Empty;

    public List<OrderItemResponseDto> Items { get; set; } = new();
}
namespace WebApplication1.Models;

public class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Pending";

    // Navigation Property
    public User User { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs;

public class UpdateOrderStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
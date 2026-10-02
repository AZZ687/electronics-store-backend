using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication1.DTOs;

public class CreateOrderDto
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public List<CreateOrderItemDto> Items { get; set; } = new();
}
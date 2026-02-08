using System.ComponentModel.DataAnnotations;
using VShop.ProductApi.Models;

namespace VShop.ProductApi.DTOs;

public class ProductDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The price is required")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "The description is required")]
    [MinLength(5)]
    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "The stock is required")]
    [Range(1, 9999)]
    public long Stock { get; set; }

    public string? ImageUrl { get; set; }
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
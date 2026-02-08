using System.ComponentModel.DataAnnotations;

namespace VShop.Web.Models;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required] public string Name { get; set; } = string.Empty;

    [Required] public decimal Price { get; set; }

    [Required] public string Description { get; set; } = string.Empty;

    [Required] public long Stock { get; set; }

    [Required] public string? ImageUrl { get; set; }

    [Range(1, 100)] public int Quantity { get; set; } = 1;
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; }
}
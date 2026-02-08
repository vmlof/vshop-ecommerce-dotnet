namespace VShop.CartApi.DTOs;

public class CartItemDto
{
    public int Id { get; set; }
    public ProductDto Product { get; set; } = new ProductDto();
    public int Quantity { get; set; } = 1;
    public int ProductId { get; set; }
    public int CartHeaderId { get; set; }
    public CartHeaderDto? CartHeader { get; set; }
}
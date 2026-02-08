namespace VShop.CartApi.DTOs;

public class CartDto
{
    public CartHeaderDto CartHeader { get; set; } = new CartHeaderDto();
    public IEnumerable<CartItemDto> CartItems { get; set; } = Enumerable.Empty<CartItemDto>();
}
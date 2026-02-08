namespace VShop.CartApi.DTOs;

public class CheckoutHeaderDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? CouponCode { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; } = 0.00m;

    public decimal Discount { get; set; } = 0.00m;

    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
    public string Telephone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string CardNumber { get; set; } = string.Empty;
    public string NameOnCard { get; set; } = string.Empty;
    public string Cvv { get; set; } = string.Empty;
    public string ExpireMonthYear { get; set; } = string.Empty;

    public int CartTotalItens { get; set; }
    public IEnumerable<CartItemDto> CartItems { get; set; } = Enumerable.Empty<CartItemDto>();
}
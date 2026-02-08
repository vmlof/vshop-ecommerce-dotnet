using VShop.CartApi.Models;

namespace VShop.CartApi.DTOs.Mappings;

public static class CartApiMappingExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        if (product == null) return null;
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            CategoryName = product.CategoryName,
        };
    }

    public static Product ToProduct(this ProductDto productDto)
    {
        if (productDto == null) return null;
        return new Product
        {
            Id = productDto.Id,
            Name = productDto.Name,
            Price = productDto.Price,
            Description = productDto.Description,
            Stock = productDto.Stock,
            ImageUrl = productDto.ImageUrl,
            CategoryName = productDto.CategoryName,
        };
    }

    public static CartHeaderDto ToDto(this CartHeader ch)
    {
        if (ch == null) return null;
        return new CartHeaderDto
        {
            Id = ch.Id,
            UserId = ch.UserId,
            CouponCode = ch.CouponCode,
        };
    }

    public static CartHeader ToCartHeader(this CartHeaderDto chDto)
    {
        if (chDto == null) return null;
        return new CartHeader
        {
            Id = chDto.Id,
            UserId = chDto.UserId,
            CouponCode = chDto.CouponCode,
        };
    }

    public static CartItemDto ToDto(this CartItem cartItem)
    {
        if (cartItem == null) return null;
        return new CartItemDto
        {
            Id = cartItem.Id,
            ProductId = cartItem.ProductId,
            Quantity = cartItem.Quantity,
            CartHeaderId = cartItem.CartHeaderId,
            Product = cartItem.Product?.ToDto(),
        };
    }

    public static CartItem ToCartItem(this CartItemDto cartItemDto)
    {
        if (cartItemDto == null) return null;
        return new CartItem
        {
            Id = cartItemDto.Id,
            ProductId = cartItemDto.ProductId,
            Quantity = cartItemDto.Quantity,
            CartHeaderId = cartItemDto.CartHeaderId,
            Product = cartItemDto.Product?.ToProduct(),
        };
    }

    public static CartDto ToDto(this Cart cart)
    {
        if (cart == null) return null;
        return new CartDto
        {
            CartHeader = cart.CartHeader?.ToDto(),
            CartItems = cart.CartItems?.Select(x => x.ToDto())
                        ?? Enumerable.Empty<CartItemDto>(),
        };
    }

    public static Cart ToCart(this CartDto cartDto)
    {
        if (cartDto == null) return null;
        return new Cart
        {
            CartHeader = cartDto.CartHeader?.ToCartHeader(),
            CartItems = cartDto.CartItems?.Select(x => x.ToCartItem()).ToList()
                        ?? Enumerable.Empty<CartItem>()
        };
    }
}
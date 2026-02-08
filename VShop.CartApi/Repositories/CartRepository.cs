using Microsoft.EntityFrameworkCore;
using VShop.CartApi.Context;
using VShop.CartApi.DTOs;
using VShop.CartApi.DTOs.Mappings;
using VShop.CartApi.Models;

namespace VShop.CartApi.Repositories;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CartDto> GetCartByUserIdAsync(string userId)
    {
        Cart cart = new Cart
        {
            // obter o header pelo UserId
            CartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId),
        };

        if (cart.CartHeader != null)
        {
            cart.CartItems = await _context.CartItems
                .Where(c => c.CartHeaderId == cart.CartHeader.Id)
                .Include(c => c.Product)
                .ToListAsync(); // Recomendado usar ToListAsync para materializar a consulta
        }
        else
        {
            return null;
        }

        return cart.ToDto();
    }

    public async Task<CartDto> UpdateCartAsync(CartDto cartDto)
    {
        Cart cart = cartDto.ToCart();

        // salva o produto no banco se ele não existir
        await SaveProductInDatabase(cartDto, cart);

        // verifica se o Cartheader é null
        var cartHeader = await _context.CartHeaders.AsNoTracking()
            .FirstOrDefaultAsync(c => c.UserId == cart.CartHeader.UserId);

        if (cartHeader == null)
        {
            // criar o header e os itens
            await CreateCartHeaderAndItems(cart);
        }
        else
        {
            // atualiza a quantidade e os itens
            await UpdateQuantityAndItems(cartDto, cart, cartHeader);
        }

        return cart.ToDto();
    }

    public async Task<bool> CleanCartAsync(string userId)
    {
        var cartHeader = await _context.CartHeaders.FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeader != null)
        {
            _context.CartItems.RemoveRange(_context.CartItems
                .Where(c => c.CartHeaderId == cartHeader.Id));

            _context.CartHeaders.Remove(cartHeader);

            await _context.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteItemCartAsync(int carItemId)
    {
        try
        {
            CartItem carItem = await _context.CartItems.FirstOrDefaultAsync(c => c.Id == carItemId);

            int total = _context.CartItems.Where(c => c.CartHeaderId == carItem.CartHeaderId).Count();

            _context.CartItems.Remove(carItem);
            await _context.SaveChangesAsync();

            if (total == 1)
            {
                var cartHeaderRemove =
                    await _context.CartHeaders.FirstOrDefaultAsync(c => c.Id == carItem.CartHeaderId);

                _context.CartHeaders.Remove(cartHeaderRemove);
                await _context.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> ApplyCouponAsync(string userId, string couponCode)
    {
        var cartHeaderApplyCoupon = await _context.CartHeaders
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeaderApplyCoupon is not null)
        {
            cartHeaderApplyCoupon.CouponCode = couponCode;

            _context.CartHeaders.Update(cartHeaderApplyCoupon);

            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteCouponAsync(string userId)
    {
        var cartHeaderDeleteCoupon = await _context.CartHeaders
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cartHeaderDeleteCoupon is not null)
        {
            cartHeaderDeleteCoupon.CouponCode = "";

            _context.CartHeaders.Update(cartHeaderDeleteCoupon);

            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    private async Task SaveProductInDatabase(CartDto cartDto, Cart cart)
    {
        // verifica se o produto já foi salvo, se não, salva
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == cartDto
            .CartItems.FirstOrDefault().ProductId);

        if (product == null)
        {
            _context.Products.Add(cart.CartItems.FirstOrDefault().Product);
            await _context.SaveChangesAsync();
        }
    }

    private async Task CreateCartHeaderAndItems(Cart cart)
    {
        // cria o CartHeader e o CartItems
        _context.CartHeaders.Add(cart.CartHeader);
        await _context.SaveChangesAsync();

        cart.CartItems.FirstOrDefault().CartHeaderId = cart.CartHeader.Id;
        cart.CartItems.FirstOrDefault().Product = null;

        _context.CartItems.Add(cart.CartItems.FirstOrDefault());

        await _context.SaveChangesAsync();
    }

    private async Task UpdateQuantityAndItems(CartDto cartDto, Cart cart, CartHeader? cartHeader)
    {
        //Se CartHeader não é null
        //verifica se CartItems possui o mesmo produto
        var cartItem = await _context.CartItems.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == cartDto.CartItems
            .FirstOrDefault()
            .ProductId && p.CartHeaderId == cartHeader.Id);

        if (cartItem is null)
        {
            //Cria o CartItems
            cart.CartItems.FirstOrDefault().CartHeaderId = cartHeader.Id;
            cart.CartItems.FirstOrDefault().Product = null;
            _context.CartItems.Add(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
        else
        {
            //Atualiza a quantidade e o CartItems
            cart.CartItems.FirstOrDefault().Product = null;
            cart.CartItems.FirstOrDefault().Quantity += cartItem.Quantity;
            cart.CartItems.FirstOrDefault().Id = cartItem.Id;
            cart.CartItems.FirstOrDefault().CartHeaderId = cartItem.CartHeaderId;
            _context.CartItems.Update(cart.CartItems.FirstOrDefault());
            await _context.SaveChangesAsync();
        }
    }
}
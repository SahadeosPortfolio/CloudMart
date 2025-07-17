using Carts.Application.DTOs;
using Carts.Application.Interfaces;
using Carts.Domain.Entities;
using Carts.Domain.Interfaces;

namespace Carts.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _repo;

    public CartService(ICartRepository repo)
    {
        _repo = repo ?? throw new ArgumentNullException(nameof(repo));
    }

    public async Task<CartDto> GetCartAsync(Guid userId)
    {
        var cart = await _repo.GetByUserIdAsync(userId) ?? new Cart(userId);

        return new CartDto
        {
            UserId = cart.UserId,
            Items = cart.Items.Select(MapToDto).ToList()
        };
    }

    public async Task AddItemAsync(Guid userId, CartItemDto itemDto)
    {
        if (itemDto == null)
            throw new ArgumentNullException(nameof(itemDto));

        var cart = await _repo.GetByUserIdAsync(userId) ?? new Cart(userId);

        var item = new CartItem(
            itemDto.ProductId,
            itemDto.ProductName,
            itemDto.UnitPrice,
            itemDto.Quantity);

        cart.AddItem(item);

        await _repo.AddOrUpdateAsync(cart);
    }

    public async Task RemoveItemAsync(Guid userId, Guid productId)
    {
        await _repo.RemoveItemAsync(userId, productId);
    }

    // Helper method for mapping
    private static CartItemDto MapToDto(CartItem item)
    {
        return new CartItemDto
        {
            ProductId = item.ProductId,
            ProductName = item.ProductName,
            UnitPrice = item.UnitPrice,
            Quantity = item.Quantity
        };
    }
}

using Carts.Application.DTOs;

namespace Carts.Application.Interfaces;

public interface ICartService
{
    Task AddItemAsync(Guid userId, CartItemDto itemDto);
    Task<CartDto> GetCartAsync(Guid userId);
    Task RemoveItemAsync(Guid userId, Guid productId);
}

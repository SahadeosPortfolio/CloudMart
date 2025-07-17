using Carts.Domain.Entities;

namespace Carts.Domain.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(Guid userId);
    Task AddOrUpdateAsync(Cart cart);
    Task RemoveItemAsync(Guid userId, Guid productId);
}

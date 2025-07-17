using Carts.Domain.Entities;
using Carts.Domain.Interfaces;
using Carts.Infrastructure.Configuration;
using Carts.Infrastructure.Persistence;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Carts.Infrastructure.Repositories;

public class CartRepository : ICartRepository
{
    private readonly IMongoCollection<CartDocument> _collection;

    public CartRepository(IOptions<MongoSettings> settings)
    {
        if (settings?.Value == null)
            throw new ArgumentNullException(nameof(settings), "MongoSettings must be provided.");

        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _collection = database.GetCollection<CartDocument>("Carts");
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId)
    {
        var doc = await _collection.Find(c => c.UserId == userId).FirstOrDefaultAsync();
        if (doc == null) return null;

        var cart = new Cart(doc.UserId);
        foreach (var item in doc.Items)
        {
            cart.AddItem(new CartItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity));
        }

        return cart;
    }

    public async Task AddOrUpdateAsync(Cart cart)
    {
        if (cart == null)
            throw new ArgumentNullException(nameof(cart));

        var doc = new CartDocument
        {
            UserId = cart.UserId,
            Items = cart.Items.Select(i => new CartItemDocument
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity
            }).ToList()
        };

        var filter = Builders<CartDocument>.Filter.Eq(c => c.UserId, cart.UserId);
        await _collection.ReplaceOneAsync(filter, doc, new ReplaceOptions { IsUpsert = true });
    }

    public async Task RemoveItemAsync(Guid userId, Guid productId)
    {
        var update = Builders<CartDocument>.Update.PullFilter(
            c => c.Items, i => i.ProductId == productId);

        await _collection.UpdateOneAsync(
            c => c.UserId == userId, update);
    }
}


namespace Carts.Domain.Entities;

public class Cart
{
    public Guid UserId { get; }
    public IReadOnlyList<CartItem> Items => _items;

    private readonly List<CartItem> _items;

    public Cart(Guid userId)
    {
        UserId = userId;
        _items = new List<CartItem>();
    }

    public void AddItem(CartItem item)
    {
        var existing = _items.FirstOrDefault(i => i.ProductId == item.ProductId);
        if (existing != null)
            existing.IncreaseQuantity(item.Quantity); // Assuming CartItem supports this
        else
            _items.Add(item);
    }

    public void RemoveItem(Guid productId)
    {
        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        if (item != null) _items.Remove(item);
    }
}

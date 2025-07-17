namespace Carts.Infrastructure.Persistence;

public class CartDocument
{
    public Guid UserId { get; set; }
    public List<CartItemDocument> Items { get; set; } = new();
}

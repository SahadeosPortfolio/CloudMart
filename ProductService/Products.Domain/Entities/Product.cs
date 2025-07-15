namespace Products.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public string Brand { get; set; }
    public List<ProductTag> Tags { get; set; }
    public List<ProductAttribute> Attributes { get; set; }
    public bool IsDeleted { get; set; }
}

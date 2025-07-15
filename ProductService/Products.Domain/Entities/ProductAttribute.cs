namespace Products.Domain.Entities;

public class ProductAttribute
{
    public Guid Id { get; set; }
    public string Key { get; set; }  // e.g., Color
    public string Value { get; set; }  // e.g., Red
}

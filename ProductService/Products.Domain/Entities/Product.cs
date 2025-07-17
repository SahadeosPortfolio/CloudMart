namespace Products.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public Category Category { get; private set; } = default!;
    public Brand Brand { get; private set; } = default!;
    public IReadOnlyCollection<ProductTag> Tags => _tags.AsReadOnly();
    public IReadOnlyCollection<ProductAttribute> Attributes => _attributes.AsReadOnly();

    private readonly List<ProductTag> _tags = new();

    private readonly List<ProductAttribute> _attributes = new();
    public bool IsDeleted { get; private set; }

    private Product() { }

    public Product(string name, string description, string imageUrl, decimal price, Category category, Brand brand)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetDescription(description);
        SetImageUrl(imageUrl);
        SetPrice(price);
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        IsDeleted = false;
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");
        Name = name.Trim();
    }

    private void SetDescription(string description)
    {
        Description = description?.Trim() ?? "";
    }

    private void SetImageUrl(string url)
    {
        ImageUrl = url?.Trim() ?? "";
    }

    private void SetPrice(decimal price)
    {
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero.");
        Price = price;
    }

    public void AddTag(ProductTag tag)
    {
        if (tag != null && !_tags.Contains(tag))
            _tags.Add(tag);
    }

    public void RemoveTag(ProductTag tag)
    {
        if (tag != null)
            _tags.Remove(tag);
    }

    public void AddAttribute(ProductAttribute attribute)
    {
        if (attribute != null && !_attributes.Contains(attribute))
            _attributes.Add(attribute);
    }

    public void RemoveAttribute(ProductAttribute attribute)
    {
        if (attribute != null)
            _attributes.Remove(attribute);
    }

    public void MarkAsDeleted() => IsDeleted = true;
}

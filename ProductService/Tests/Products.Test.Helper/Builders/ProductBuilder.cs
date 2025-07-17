using Products.Domain.Entities;

namespace Products.Test.Helper.Builders;

public class ProductBuilder
{
    private string _name = "Default Product Name";
    private string _description = "Default Product Description";
    private string _imageUrl = "http://example.com/image.jpg";
    private decimal _price = 9.99M;
    private string _category = "Default Category";
    private string _brand = "Default Brand";

    public ProductBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    public ProductBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }
    public ProductBuilder WithImageUrl(string imageUrl)
    {
        _imageUrl = imageUrl;
        return this;
    }
    public ProductBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }
    public ProductBuilder WithCategory(string category)
    {
        _category = category;
        return this;
    }
    public ProductBuilder WithBrand(string brand)
    {
        _brand = brand;
        return this;
    }
    public Product Build()
    {
        var category = new Category(_category);
        var brand = new Brand(_brand);

        var product = new Product(_name, _description, _imageUrl, _price, category, brand);

        return product;
    }
}

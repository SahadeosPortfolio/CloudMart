using Products.Application.Dtos;

namespace Products.Test.Helper.Builders
{
    public class ProductCreateRequestDtoBuilder
    {
        private string _name = "Default Product Name";
        private string _description = "Default Product Description";
        private string _imageUrl = "http://example.com/image.jpg";
        private decimal _price = 9.99M;
        private string _category = "Default Category";
        private string _brand = "Default Brand";
        private List<string>? _tags;
        private Dictionary<string, string>? _attributes;
        public ProductCreateRequestDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        public ProductCreateRequestDtoBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }
        public ProductCreateRequestDtoBuilder WithImageUrl(string imageUrl)
        {
            _imageUrl = imageUrl;
            return this;
        }
        public ProductCreateRequestDtoBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }
        public ProductCreateRequestDtoBuilder WithCategory(string category)
        {
            _category = category;
            return this;
        }
        public ProductCreateRequestDtoBuilder WithBrand(string brand)
        {
            _brand = brand;
            return this;
        }
        public ProductCreateRequestDtoBuilder WithTags(List<string> tags)
        {
            _tags = tags;
            return this;
        }
        public ProductCreateRequestDtoBuilder WithAttributes(Dictionary<string, string> attributes)
        {
            _attributes = attributes;
            return this;
        }
        public ProductCreateRequestDto Build()
        {
            return new ProductCreateRequestDto(
                _name,
                _description,
                _imageUrl,
                _price,
                _category,
                _brand,
                _tags,
                _attributes
            );
        }
    }
}

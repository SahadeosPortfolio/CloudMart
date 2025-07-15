using Products.Application.Dtos;

namespace Products.Test.Helper.Builders
{
    public class ProductResponseDtoBuilder
    {
        private Guid _id = Guid.NewGuid();
        private string _name = "Default Product Name";
        private string _description = "Default Product Description";
        private string _imageUrl = "http://example.com/image.jpg";
        private decimal _price = 9.99M;
        private string _category = "Default Category";
        private string _brand = "Default Brand";

        public ProductResponseDtoBuilder WithId(Guid id)
        {
            _id = id;
            return this;
        }
        public ProductResponseDtoBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public ProductResponseDtoBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public ProductResponseDtoBuilder WithImageUrl(string imageUrl)
        {
            _imageUrl = imageUrl;
            return this;
        }

        public ProductResponseDtoBuilder WithPrice(decimal price)
        {
            _price = price;
            return this;
        }

        public ProductResponseDtoBuilder WithCategory(string category)
        {
            _category = category;
            return this;
        }

        public ProductResponseDtoBuilder WithBrand(string brand)
        {
            _brand = brand;
            return this;
        }

        public ProductResponseDto Build()
        {
            return new ProductResponseDto(
                _id,
                _name,
                _description,
                _imageUrl,
                _price,
                _category,
                _brand
            );
        }
    }
}

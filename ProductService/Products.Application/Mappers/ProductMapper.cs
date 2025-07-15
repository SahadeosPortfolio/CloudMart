using Products.Application.Dtos;
using Products.Domain.Entities;

namespace Products.Application.Mappers;

public static class ProductMapper
{
    public static Product MapToDomain(ProductCreateRequestDto productCreateRequestDto)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = productCreateRequestDto.Name,
            Description = productCreateRequestDto.Description,
            ImageUrl = productCreateRequestDto.ImageUrl,
            Price = productCreateRequestDto.Price,
            Category = productCreateRequestDto.Category,
            Brand = productCreateRequestDto.Brand,
            Tags = productCreateRequestDto.Tags?.Select(tag => new ProductTag { Id = Guid.NewGuid(), Name = tag }).ToList() ?? new List<ProductTag>(),
            Attributes = productCreateRequestDto.Attributes?.Select(attr => new ProductAttribute
            {
                Id = Guid.NewGuid(),
                Key = attr.Key,
                Value = attr.Value
            }).ToList() ?? new List<ProductAttribute>()
        };
    }

    public static ProductResponseDto MapToDto(Product product)
    {
        return new ProductResponseDto(
                product.Id,
                product.Name,
                product.Description,
                product.ImageUrl,
                product.Price,
                product.Category,
                product.Brand
            );
    }

    public static Product MapToDomain(ProductUpdateRequestDto productUpdateRequestDto)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = productUpdateRequestDto.Name,
            Description = productUpdateRequestDto.Description,
            ImageUrl = productUpdateRequestDto.ImageUrl,
            Price = productUpdateRequestDto.Price,
            Category = productUpdateRequestDto.Category,
            Brand = productUpdateRequestDto.Brand,
            Tags = productUpdateRequestDto.Tags?.Select(tag => new ProductTag { Id = Guid.NewGuid(), Name = tag }).ToList() ?? new List<ProductTag>(),
            Attributes = productUpdateRequestDto.Attributes?.Select(attr => new ProductAttribute
            {
                Id = Guid.NewGuid(),
                Key = attr.Key,
                Value = attr.Value
            }).ToList() ?? new List<ProductAttribute>()
        };
    }
}

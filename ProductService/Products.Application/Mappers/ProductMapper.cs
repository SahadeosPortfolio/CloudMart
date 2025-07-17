using Products.Application.Dtos;
using Products.Domain.Entities;

namespace Products.Application.Mappers;

public static class ProductMapper
{
    public static Product MapToDomain(ProductCreateRequestDto dto)
    {
        var product = new Product(
            name: dto.Name,
            description: dto.Description,
            imageUrl: dto.ImageUrl,
            price: dto.Price,
            category: new Category(dto.Category),
            brand: new Brand(dto.Brand)
        );

        if (dto.Tags != null)
        {
            foreach (var tag in dto.Tags)
                product.AddTag(new ProductTag(tag));
        }

        if (dto.Attributes != null)
        {
            foreach (var attr in dto.Attributes)
                product.AddAttribute(new ProductAttribute(attr.Key, attr.Value));
        }

        return product;
    }

    public static ProductResponseDto MapToDto(Product product)
    {
        return new ProductResponseDto(
                product.Id,
                product.Name,
                product.Description,
                product.ImageUrl,
                product.Price,
                product.Category.Name,
                product.Brand.Name
            );
    }

    public static Product MapToDomain(ProductUpdateRequestDto dto)
    {
        var product = new Product(
            name: dto.Name,
            description: dto.Description,
            imageUrl: dto.ImageUrl,
            price: dto.Price,
            category: new Category(dto.Category),
            brand: new Brand(dto.Brand)
        );

        if (dto.Tags != null)
        {
            foreach (var tag in dto.Tags)
                product.AddTag(new ProductTag(tag));
        }

        if (dto.Attributes != null)
        {
            foreach (var attr in dto.Attributes)
                product.AddAttribute(new ProductAttribute(attr.Key, attr.Value));
        }

        return product;
    }
}

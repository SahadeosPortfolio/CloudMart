using Products.Application.Dtos;
using Products.Application.Interfaces;
using Products.Application.Mappers;
using Products.Domain.Interfaces;

namespace Products.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PaginatedResponse<ProductResponseDto>> GetAllAsync(ProductQueryParameters query)
    {
        var products = await _productRepository.GetAllAsync(
            query.SearchTerm,
            query.Category,
            query.Brand,
            query.MinPrice,
            query.MaxPrice,
            query.Tags,
            query.Attributes,
            query.SortBy,
            query.Ascending,
            query.Page,
            query.PageSize
        );

        return PaginatedResponseMapper.MapToPaginatedResponse(
            products.Select(ProductMapper.MapToDto),
            query.Page,
            query.PageSize
        );
    }

    public async Task<ProductResponseDto?> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        return product is null
            ? null
            : ProductMapper.MapToDto(product);
    }

    public async Task<ProductResponseDto?> CreateAsync(ProductCreateRequestDto productCreateRequestDto)
    {
        var product = ProductMapper.MapToDomain(productCreateRequestDto);
        var createdProduct = await _productRepository.AddAsync(product);

        return ProductMapper.MapToDto(createdProduct);
    }

    public async Task<bool> UpdateAsync(Guid productId, ProductUpdateRequestDto productUpdateRequestDto)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product is null || product.IsDeleted)
            return false;

        await _productRepository.UpdateAsync(ProductMapper.MapToDomain(productUpdateRequestDto));

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product is null || product.IsDeleted)
            return false;

        product.IsDeleted = true;
        await _productRepository.UpdateAsync(product);

        return true;
    }
}

using Products.Application.Dtos;

namespace Products.Application.Interfaces;

public interface IProductService
{
    Task<ProductResponseDto?> GetByIdAsync(Guid id);
    Task<PaginatedResponse<ProductResponseDto>> GetAllAsync(ProductQueryParameters productQueryParameters);
    Task<ProductResponseDto?> CreateAsync(ProductCreateRequestDto productCreateRequestDto);
    Task<bool> UpdateAsync(Guid productId, ProductUpdateRequestDto productUpdateRequestDto);
    Task<bool> DeleteAsync(Guid id);
}

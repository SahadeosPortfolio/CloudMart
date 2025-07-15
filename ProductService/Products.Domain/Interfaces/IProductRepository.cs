using Products.Domain.Entities;

namespace Products.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync(string? searchTerm,
                                           string? category,
                                           string? brand,
                                           decimal? minPrice,
                                           decimal? maxPrice,
                                           List<string>? tags,
                                           Dictionary<string, string>? attributes,
                                           string? sortBy,
                                           bool ascending,
                                           int page,
                                           int pageSize);

    Task<Product> AddAsync(Product product);
    Task UpdateAsync(Product product);
}

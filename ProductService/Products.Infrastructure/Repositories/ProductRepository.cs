using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Infrastructure.Data;

namespace Products.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(string? searchTerm,
                                                    string? category,
                                                    string? brand,
                                                    decimal? minPrice,
                                                    decimal? maxPrice,
                                                    List<string>? tags,
                                                    Dictionary<string, string>? attributes,
                                                    string? sortBy,
                                                    bool ascending,
                                                    int page,
                                                    int pageSize)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;

        var query = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Tags)
            .Include(p => p.Attributes)
            .AsQueryable();

        query = ApplyFilters(query, searchTerm, category, brand, minPrice, maxPrice, tags, attributes);
        query = ApplySorting(query, sortBy, ascending);

        return await query
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        var product = await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Include(p => p.Tags)
            .Include(p => p.Attributes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            throw new KeyNotFoundException($"Product with ID {id} not found.");

        return product;
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return product;
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }

    #region Private Methods

    private IQueryable<Product> ApplyFilters(
    IQueryable<Product> query,
    string? searchTerm,
    string? category,
    string? brand,
    decimal? minPrice,
    decimal? maxPrice,
    List<string>? tags,
    Dictionary<string, string>? attributes)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(p =>
                p.Name.Contains(searchTerm) ||
                p.Brand.Name.Contains(searchTerm));  // p.Brand is an entity
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => p.Category.Name == category);
        }

        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(p => p.Brand.Name == brand);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        if (tags?.Count > 0)
        {
            query = query.Where(p => tags.All(tag => p.Tags.Any(t => t.Name == tag)));
        }

        if (attributes?.Count > 0)
        {
            query = query.Where(p =>
                attributes.All(attr =>
                    p.Attributes.Any(pa => pa.Key == attr.Key && pa.Value == attr.Value)));
        }

        return query;
    }

    private IQueryable<Product> ApplySorting(IQueryable<Product> query, string? sortBy, bool ascending)
    {
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLowerInvariant() switch
            {
                "name" => ascending ? query.OrderBy(p => p.Name) : query.OrderByDescending(p => p.Name),
                "price" => ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                "brand" => ascending ? query.OrderBy(p => p.Brand) : query.OrderByDescending(p => p.Brand),
                "category" => ascending ? query.OrderBy(p => p.Category) : query.OrderByDescending(p => p.Category),
                _ => query
            };
        }

        return query;
    }

    #endregion
}

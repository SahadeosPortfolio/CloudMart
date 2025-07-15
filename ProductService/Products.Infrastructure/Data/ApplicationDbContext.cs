using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductTag> ProductTags { get; set; }
    public DbSet<ProductAttribute> ProductAttributes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configure your entities here if needed

        ConfigureProduct(modelBuilder);
        ConfigureTag(modelBuilder);
        ConfigureProductAttribute(modelBuilder);
    }

    private void ConfigureProduct(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.Brand);
            entity.HasIndex(p => p.Category);
            entity.HasIndex(p => p.Price);
        });
    }

    private void ConfigureTag(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.HasIndex(pt => pt.Id);
        });
    }

    private void ConfigureProductAttribute(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductAttribute>(entity =>
        {
            entity.HasIndex(p => new { p.Id, p.Value });
        });
    }
}

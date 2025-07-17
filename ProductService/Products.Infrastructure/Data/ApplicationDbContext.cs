using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;

namespace Products.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductTag> ProductTags { get; set; } = null!;
    public DbSet<ProductAttribute> ProductAttributes { get; set; } = null!;
    public DbSet<Brand> Brands { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureProduct(modelBuilder);
        ConfigureBrand(modelBuilder);
        ConfigureCategory(modelBuilder);
    }

    private void ConfigureProduct(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Name);
            entity.HasIndex(p => p.Price);

            // Use shadow properties for foreign keys
            entity.Property<Guid>("BrandId");
            entity.Property<Guid>("CategoryId");

            entity.HasOne(p => p.Brand)
                  .WithMany()
                  .HasForeignKey("BrandId")
                  .IsRequired();

            entity.HasOne(p => p.Category)
                  .WithMany()
                  .HasForeignKey("CategoryId")
                  .IsRequired();

            // Tags
            entity.OwnsMany(p => p.Tags, tags =>
            {
                tags.WithOwner().HasForeignKey("ProductId");
                tags.Property(t => t.Name).IsRequired().HasMaxLength(50);
                tags.ToTable("ProductTags");
                tags.HasKey("ProductId", "Name");
            });

            // Attributes
            entity.OwnsMany(p => p.Attributes, attrs =>
            {
                attrs.WithOwner().HasForeignKey("ProductId");
                attrs.Property(a => a.Key).IsRequired().HasMaxLength(50);
                attrs.Property(a => a.Value).IsRequired().HasMaxLength(100);
                attrs.ToTable("ProductAttributes");
                attrs.HasKey("ProductId", "Key");
            });
        });
    }

    private void ConfigureBrand(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(entity =>
        {
            entity.ToTable("Brands");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Name)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.HasIndex(b => b.Name).IsUnique();
        });
    }

    private void ConfigureCategory(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(100);
            entity.HasIndex(c => c.Name).IsUnique();
        });
    }
}

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Products.Domain.Entities;
using Products.Infrastructure.Data;
using System.Collections.Generic;

namespace Products.Integration.Tests.TestUtilities;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public List<Product> SeededProducts { get; private set; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 🔧 Remove existing ApplicationDbContext registrations
            var descriptorsToRemove = services
               .Where(d => d.ServiceType == typeof(ApplicationDbContext) ||
                           d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>) ||
                           d.ServiceType.FullName?.Contains("Microsoft.EntityFrameworkCore") == true)
               .ToList();

            foreach (var descriptor in descriptorsToRemove)
                services.Remove(descriptor);

            // ✅ Use in-memory DB for test
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TestDb")); // optionally use Guid.NewGuid().ToString() for per-test DB

            // 🔄 Build and seed DB
            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            SeedTestData(db);
        });
    }

    #region Private Methods
    private void SeedTestData(ApplicationDbContext db)
    {
        var product1 = CreateProduct(
            name: "Test Product 1",
            category: "Test Category1",
            brand: "Test Brand1",
            imageUrl: "https://example.com/image1.jpg",
            price: 9.99m
        );

        var product2 = CreateProduct(
            name: "Test Product 2",
            category: "Test Category2",
            brand: "Test Brand2",
            imageUrl: "https://example.com/image2.jpg",
            price: 19.99m
        );

        db.Products.AddRange(product1, product2);
        db.SaveChanges();

        SeededProducts.AddRange(new[] { product1, product2 });
    }

    private Product CreateProduct(string name, string category, string brand, string imageUrl, decimal price)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = $"Description for {name}",
            Price = price,
            Category = category,
            Brand = brand,
            ImageUrl = imageUrl
        };
    } 

    #endregion
}
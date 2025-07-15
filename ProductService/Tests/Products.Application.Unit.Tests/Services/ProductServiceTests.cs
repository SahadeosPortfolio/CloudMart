using Moq;
using Products.Application.Dtos;
using Products.Application.Services;
using Products.Domain.Entities;
using Products.Domain.Interfaces;

namespace Products.Application.Unit.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productService = new ProductService(_productRepositoryMock.Object);
    }


    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var expectedProduct = new Product { Id = productId, Name = "Test Product" };
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(expectedProduct);
        // Act
        var result = await _productService.GetByIdAsync(productId);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProduct.Id, result.Id);
        Assert.Equal(expectedProduct.Name, result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product?)null);
        // Act
        var result = await _productService.GetByIdAsync(productId);
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedProduct_WhenProductIsValid()
    {
        // Arrange
        var productCreateRequestDto = new ProductCreateRequestDto("TestProductName", "testProductDescription", "testImageUrl", 11.2M, "testProductCategory", "testProductBrand");
        var createdProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = productCreateRequestDto.Name,
            Description = productCreateRequestDto.Description,
            ImageUrl = productCreateRequestDto.ImageUrl,
            Price = productCreateRequestDto.Price,
            Category = productCreateRequestDto.Category,
            Brand = productCreateRequestDto.Brand
        };

        _productRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Product>())).ReturnsAsync(createdProduct);
        // Act
        var result = await _productService.CreateAsync(productCreateRequestDto);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(createdProduct.Id, result.Id);
        Assert.Equal(createdProduct.Name, result.Name);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedProducts_WhenProductsExist()
    {
        // Arrange
        var queryParameters = new ProductQueryParameters { Page = 1, PageSize = 10 };
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Product1" },
            new Product { Id = Guid.NewGuid(), Name = "Product2" }
        };

        _productRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<string>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<decimal?>(),
                                                              It.IsAny<decimal?>(),
                                                              It.IsAny<List<string>>(),
                                                              It.IsAny<Dictionary<string, string>>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<bool>(),
                                                              queryParameters.Page,
                                                              queryParameters.PageSize))
            .ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllAsync(queryParameters);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnEmptyList_WhenNoProductsExist()
    {
        // Arrange
        var queryParameters = new ProductQueryParameters { Page = 1, PageSize = 10 };
        _productRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<string>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<decimal?>(),
                                                              It.IsAny<decimal?>(),
                                                              It.IsAny<List<string>>(),
                                                              It.IsAny<Dictionary<string, string>>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<bool>(),
                                                              queryParameters.Page,
                                                              queryParameters.PageSize))
            .ReturnsAsync(new List<Product>());

        // Act
        var result = await _productService.GetAllAsync(queryParameters);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnPaginatedResponse_WhenProductsExistWithPagination()
    {
        // Arrange
        var queryParameters = new ProductQueryParameters { Page = 1, PageSize = 10 };
        var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Product1" },
            new Product { Id = Guid.NewGuid(), Name = "Product2" }
        };

        _productRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<string>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<decimal?>(),
                                                              It.IsAny<decimal?>(),
                                                              It.IsAny<List<string>>(),
                                                              It.IsAny<Dictionary<string, string>>(),
                                                              It.IsAny<string>(),
                                                              It.IsAny<bool>(),
                                                              queryParameters.Page,
                                                              queryParameters.PageSize))
            .ReturnsAsync(products);

        // Act
        var result = await _productService.GetAllAsync(queryParameters);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(queryParameters.Page, result.Page);
        Assert.Equal(queryParameters.PageSize, result.PageSize);
    }
}

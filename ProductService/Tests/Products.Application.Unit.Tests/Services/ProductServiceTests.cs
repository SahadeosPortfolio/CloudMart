using Moq;
using Products.Application.Dtos;
using Products.Application.Services;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Test.Helper.Builders;

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
        var expectedProduct = new ProductBuilder().WithName("testProductBrand")
                                                  .Build();
        var productId = expectedProduct.Id;

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
        var createdProduct = new ProductBuilder()
            .WithName(productCreateRequestDto.Name)
            .WithDescription(productCreateRequestDto.Description)
            .WithImageUrl(productCreateRequestDto.ImageUrl)
            .WithPrice(productCreateRequestDto.Price)
            .WithCategory(productCreateRequestDto.Category)
            .WithBrand(productCreateRequestDto.Brand)
            .Build();

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
            new ProductBuilder().WithName("Product1").Build(),
            new ProductBuilder().WithName("Product2").Build()
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
            new ProductBuilder().WithName("Product1").Build(),
            new ProductBuilder().WithName("Product2").Build()
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

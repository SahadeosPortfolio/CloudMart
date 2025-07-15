using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.Api.Controllers;
using Products.Application.Dtos;
using Products.Application.Interfaces;

namespace Products.Api.Unit.Tests.Controller;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _productServiceMock;
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _controller = new ProductsController(_productServiceMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOkResult_WhenProductsExist()
    {
        // Arrange
        var productQueryParameters = new ProductQueryParameters();
        var products = new PaginatedResponse<ProductResponseDto>
        {
            Items = new List<ProductResponseDto>(),
            TotalCount = 0,
            PageSize = 10,
            Page = 1
        };

        _productServiceMock.Setup(s => s.GetAllAsync(productQueryParameters)).ReturnsAsync(products);
        // Act
        var result = await _controller.GetAllAsync(productQueryParameters);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOkResult_WhenProductExists()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new ProductResponseDto(productId, "testProduct", "testDescription", "testImageUrl", 11.0m, "testCategory", "testBrand");
        _productServiceMock.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync(product);
        // Act
        var result = await _controller.GetByIdAsync(productId);
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(product, okResult.Value);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _productServiceMock.Setup(s => s.GetByIdAsync(productId)).ReturnsAsync((ProductResponseDto?)null);
        // Act
        var result = await _controller.GetByIdAsync(productId);
        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedResult_WhenProductIsValid()
    {
        // Arrange
        var productCreateRequestDto = new ProductCreateRequestDto("testProduct", "testDescription", "testImageUrl", 11.0m, "testCategory", "testBrand");
        var createdProduct = new ProductResponseDto(Guid.NewGuid(), productCreateRequestDto.Name, productCreateRequestDto.Description, productCreateRequestDto.ImageUrl, productCreateRequestDto.Price, productCreateRequestDto.Category, productCreateRequestDto.Brand);

        _productServiceMock.Setup(s => s.CreateAsync(productCreateRequestDto)).ReturnsAsync(createdProduct);
        // Act
        var result = await _controller.CreateAsync(productCreateRequestDto);
        // Assert
        var createdResult = Assert.IsType<CreatedAtRouteResult>(result);
        Assert.Equal("GetProductById", createdResult.RouteName);
        Assert.Equal(createdProduct, createdResult.Value);
    }
}
using Microsoft.AspNetCore.Mvc;
using Products.Application.Dtos;
using Products.Integration.Tests.TestUtilities;
using Products.Test.Helper.Builders;
using System.Net.Http.Json;
using System.Text.Json;

namespace Products.Integration.Tests;

public class ProductsControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;
    private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ProductsControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithPaginatedResponse()
    {
        // Arrange
        const string requestUri = "/api/products";

        // Act
        var response = await _client.GetAsync(requestUri);

        // Assert - Status Code
        response.EnsureSuccessStatusCode();

        // Assert - Content
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content), "Expected non-empty response content.");

        var paginatedResponse = JsonSerializer.Deserialize<PaginatedResponse<ProductResponseDto>>(content, _jsonOptions);

        Assert.NotNull(paginatedResponse);
        Assert.NotNull(paginatedResponse.Items);
        Assert.NotEmpty(paginatedResponse.Items);
        Assert.True(paginatedResponse.Items.Count > 0, "Expected at least one product in the response.");
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WithProductResponseDto()
    {
        // Arrange
        var seededProduct = _factory.SeededProducts.FirstOrDefault();
        Assert.NotNull(seededProduct); // Ensures test data is available
        var requestUri = $"/api/products/{seededProduct.Id}";

        // Act
        var response = await _client.GetAsync(requestUri);

        // Assert - Status
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content), "Expected non-empty response content.");

        var product = JsonSerializer.Deserialize<ProductResponseDto>(content, _jsonOptions);

        Assert.NotNull(product);
        Assert.Equal(seededProduct.Id, product.Id);
        Assert.Equal(seededProduct.Name, product.Name);
    }

    [Fact]
    public async Task CreateAsync_ReturnsCreatedResult_WithProductResponseDto()
    {
        // Arrange
        var newProduct = new ProductCreateRequestDtoBuilder()
            .WithName("New Test Product")
            .WithDescription("This is a new test product for integration testing.")
            .WithCategory("Test Category")
            .WithBrand("Test Brand")
            .WithImageUrl("https://example.com/new-image.jpg")
            .WithPrice(29.99m)
            .Build();

        const string requestUri = "/api/products";

        // Act
        var response = await _client.PostAsJsonAsync(requestUri, newProduct);

        // Assert - Status
        Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content), "Expected non-empty response content.");

        var createdProduct = JsonSerializer.Deserialize<ProductResponseDto>(content, _jsonOptions);

        Assert.NotNull(createdProduct);
        Assert.Equal(newProduct.Name, createdProduct.Name);
        Assert.Equal(newProduct.Price, createdProduct.Price);
        Assert.Equal(newProduct.Category, createdProduct.Category);
    }

    [Fact]
    public async Task CreateAsync_InvalidInput_ReturnsBadRequest()
    {
        // Arrange
        var invalidProduct = new ProductCreateRequestDtoBuilder()
            .WithName("") // Empty name should trigger validation error
            .WithDescription("This product has an empty name.")
            .WithCategory("Test Category")
            .WithBrand("Test Brand")
            .WithImageUrl("https://example.com/invalid-image.jpg")
            .WithPrice(29.99m)
            .Build();
        const string requestUri = "/api/products";
        // Act
        var response = await _client.PostAsJsonAsync(requestUri, invalidProduct);
        // Assert - Status
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrWhiteSpace(content), "Expected non-empty response content.");
        //deserialize the error response to check for specific validation errors
        var errorResponse = JsonSerializer.Deserialize<ValidationProblemDetails>(content, _jsonOptions);
        Assert.NotNull(errorResponse);
        Assert.Contains("Product name is required.", errorResponse.Errors["Name"]);
    }
}

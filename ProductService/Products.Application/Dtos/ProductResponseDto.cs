namespace Products.Application.Dtos;

public record ProductResponseDto(
    Guid Id,
    string Name,
    string Description,
    string ImageUrl,
    decimal Price,
    string Category,
    string Brand
);

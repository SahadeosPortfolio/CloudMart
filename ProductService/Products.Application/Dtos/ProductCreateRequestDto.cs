namespace Products.Application.Dtos;

public record ProductCreateRequestDto(
    string Name,
    string Description,
    string ImageUrl,
    decimal Price,
    string Category,
    string Brand,
    List<string>? Tags = null,
    Dictionary<string, string>? Attributes = null
);

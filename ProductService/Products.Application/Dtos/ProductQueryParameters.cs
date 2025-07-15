namespace Products.Application.Dtos;

public record ProductQueryParameters
{
    public string? SearchTerm { get; init; } // Name, SKU, Brand
    public string? Category { get; init; }
    public string? Brand { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public string? SortBy { get; init; }
    public bool Ascending { get; init; } = true;
    public int Page { get; init; }
    public int PageSize { get; init; }
    public List<string>? Tags { get; init; }

    //todo : attributes are not supported yet, but can be added later
    public Dictionary<string, string>? Attributes { get; init; }  // e.g., { "Color": "Red" } 
}

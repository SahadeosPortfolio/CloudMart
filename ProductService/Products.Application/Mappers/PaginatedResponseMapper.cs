using Products.Application.Dtos;

namespace Products.Application.Mappers;

public static class PaginatedResponseMapper
{
    public static PaginatedResponse<T> MapToPaginatedResponse<T>(
        IEnumerable<T> items, int page, int pageSize)
    {
        var itemList = items.ToList();

        return new PaginatedResponse<T>
        {
            Items = itemList,
            Page = page,
            PageSize = pageSize,
            TotalCount = itemList.Count
        };
    }
}

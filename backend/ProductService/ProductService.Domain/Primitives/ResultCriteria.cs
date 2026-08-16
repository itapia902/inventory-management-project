namespace ProductService.Domain.Primitives;

public record ResultCriteria<T>(
    IReadOnlyList<T> items,
    int Page,
    int PageSize,
    int TotalItems
    )
{
    public int TotalPages => TotalItems == 0 ? 0 : (TotalItems + PageSize - 1) / PageSize;
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;

}

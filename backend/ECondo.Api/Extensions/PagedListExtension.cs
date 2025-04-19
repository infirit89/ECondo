using ECondo.Domain.Shared;

namespace ECondo.Api.Extensions;

public sealed record PagedListResponse<T>(
    IEnumerable<T> Items, 
    int CurrentPage, 
    int TotalPages, 
    int PageSize,
    int TotalCount,
    bool HasPrevious,
    bool HasNext);

public static class PagedListExtension
{
    public static PagedListResponse<T> ToPagedListResponse<T>(this PagedList<T> pagedList)
        => new(
            pagedList.ToArray(), 
            pagedList.CurrentPage, 
            pagedList.TotalPages,
            pagedList.PageSize,
            pagedList.TotalCount,
            pagedList.HasPrevious,
            pagedList.HasNext);

}
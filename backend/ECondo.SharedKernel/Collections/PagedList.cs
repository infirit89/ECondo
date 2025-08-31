namespace ECondo.SharedKernel.Collections;

public class PagedList<T>(IEnumerable<T> items, int totalItemCount, int pageNumber, int pageSize)
    : List<T>(items)
{
    public int CurrentPage { get; set; } = pageNumber;
    public int TotalPages { get; set; } = (int)Math.Ceiling(totalItemCount / (double)pageSize);
    public int PageSize { get; set; } = pageSize;
    public int TotalCount { get; set; } = totalItemCount;
    public bool HasPrevious => CurrentPage > 0;
    public bool HasNext => CurrentPage < TotalPages - 1;
}

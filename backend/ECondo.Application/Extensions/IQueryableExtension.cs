using ECondo.SharedKernel.Collections;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Extensions;

public static class QueryableExtension
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> queryable,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var count = await queryable.CountAsync(cancellationToken: cancellationToken);
        
        var data = await queryable
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToArrayAsync(cancellationToken: cancellationToken);

        return new PagedList<T>(data, count, page, pageSize);
    }
}
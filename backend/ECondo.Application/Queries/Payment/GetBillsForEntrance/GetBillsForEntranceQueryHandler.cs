using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Payment.GetBillsForEntrance;

internal sealed class GetBillsForEntranceQueryHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetBillsForEntranceQuery, PagedList<BillResult>>
{
    public async Task<Result<PagedList<BillResult>, Error>> Handle(GetBillsForEntranceQuery request, CancellationToken cancellationToken)
    {
        var bills = await dbContext
            .Bills
            .AsNoTracking()
            .Where(b => b.Entrance.Id == request.EntranceId)
            .Select(e => new BillResult(e.Title, e.Description, e.Amount, e.RecurringInterval, e.StartDate, e.EndDate))
            .ToPagedListAsync(request.Page, request.PageSize, cancellationToken: cancellationToken);
        
        return Result<PagedList<BillResult>, Error>.Ok(bills);
    }
}
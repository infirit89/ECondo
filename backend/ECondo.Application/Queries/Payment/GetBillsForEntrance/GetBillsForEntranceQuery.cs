using ECondo.Application.Policies;
using ECondo.Application.Policies.EntranceManager;
using ECondo.Domain.Payments;
using ECondo.Domain.Shared;

namespace ECondo.Application.Queries.Payment.GetBillsForEntrance;

public sealed record BillResult(
    string Title,
    string? Description,
    decimal Amount,
    RecurringInterval? Interval,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate);

public sealed record GetBillsForEntranceQuery(Guid BuildingId, string EntranceNumber, int Page, int PageSize) : 
    IQuery<PagedList<BillResult>>, 
    IIsEntranceManager;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.Domain.Payments;
using ECondo.SharedKernel.Collections;

namespace ECondo.Application.Queries.Payment.GetBillsForEntrance;

public sealed record BillResult(
    string Title,
    string? Description,
    decimal Amount,
    RecurringInterval? Interval,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate);

public sealed record GetBillsForEntranceQuery(
    Guid EntranceId, int Page, int PageSize) :
    IQuery<PagedList<BillResult>>, ICanRead<Entrance>
{
    Guid? IResourcePolicy.ResourceId => EntranceId;
}
using ECondo.Application.Policies;
using ECondo.Domain.Shared;

namespace ECondo.Application.Queries.Payment.GetBillsForEntrance;

public sealed record BillResult(string Title, string? Description, decimal Amount);

public sealed record GetBillsForEntranceQuery(Guid BuildingId, string EntranceNumber, int Page, int PageSize) : 
    IQuery<PagedList<BillResult>>, 
    IRequireEntranceManager;
using ECondo.Application.Policies;

namespace ECondo.Application.Queries.Payment.GetStripeLoginLink;

public sealed record GetEntranceStripeLoginLinkQuery(
    Guid BuildingId, 
    string EntranceNumber) : 
    IQuery<string>, IRequireEntranceManager;
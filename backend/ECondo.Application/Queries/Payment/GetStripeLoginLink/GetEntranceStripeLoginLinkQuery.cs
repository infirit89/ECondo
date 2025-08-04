using ECondo.Application.Authorization.Policies.EntranceManager;

namespace ECondo.Application.Queries.Payment.GetStripeLoginLink;

public sealed record GetEntranceStripeLoginLinkQuery(
    Guid BuildingId, 
    string EntranceNumber) : 
    IQuery<string>, ICanSeeEntrance;
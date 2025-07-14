using ECondo.Application.Policies;
using ECondo.Application.Policies.EntranceManager;
using ECondo.Application.Services;

namespace ECondo.Application.Queries.Payment.CheckStripeStatus;

public record CheckEntranceStripeStatusQuery(
    Guid BuildingId, 
    string EntranceNumber): 
    IQuery<StripeStatus>, IIsEntranceManager;
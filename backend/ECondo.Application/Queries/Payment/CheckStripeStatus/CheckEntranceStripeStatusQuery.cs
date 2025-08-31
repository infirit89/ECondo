using ECondo.Application.Services;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Queries.Payment.CheckStripeStatus;

public record CheckEntranceStripeStatusQuery(
    Guid EntranceId) :
    IQuery<StripeStatus>, ICanRead<Entrance>
{
    Guid? IResourcePolicy.ResourceId => EntranceId;
}
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Queries.Payment.GetStripeLoginLink;

public sealed record GetEntranceStripeLoginLinkQuery(
    Guid EntranceId) :
    IQuery<string>, ICanRead<Entrance>
{
    Guid? IResourcePolicy.ResourceId => EntranceId;
}
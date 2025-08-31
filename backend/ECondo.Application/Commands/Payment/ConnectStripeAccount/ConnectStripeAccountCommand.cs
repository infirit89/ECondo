using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;

namespace ECondo.Application.Commands.Payment.ConnectStripeAccount;

public record ConnectStripeAccountCommand(
    Guid EntranceId,
    string ReturnUri)
    : ICommand<string>, ICanUpdate<Entrance>
{
    Guid? IResourcePolicy.ResourceId => EntranceId;
}
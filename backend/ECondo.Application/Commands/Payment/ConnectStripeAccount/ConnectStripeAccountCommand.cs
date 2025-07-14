using ECondo.Application.Policies;
using ECondo.Application.Policies.EntranceManager;

namespace ECondo.Application.Commands.Payment.ConnectStripeAccount;

public record ConnectStripeAccountCommand(
    Guid BuildingId, 
    string EntranceNumber,
    string ReturnUri) 
    : ICommand<string>, IIsEntranceManager;
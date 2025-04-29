using ECondo.Application.Policies;

namespace ECondo.Application.Commands.Payment.ConnectStripeAccount;

public record ConnectStripeAccountCommand(
    Guid BuildingId, 
    string EntranceNumber,
    string ReturnUri) 
    : ICommand<string>, IRequireEntranceManager;
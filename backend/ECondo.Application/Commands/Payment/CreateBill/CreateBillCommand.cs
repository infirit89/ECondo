using ECondo.Application.Policies;
using ECondo.Domain.Payments;

namespace ECondo.Application.Commands.Payment.CreateBill;

public sealed record CreateBillCommand(
    Guid BuildingId,
    string EntranceNumber,
    string Title, 
    string? Description, 
    decimal Amount,
    bool IsRecurring,
    RecurringInterval? RecurringInterval,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate)
    : ICommand<Guid>, IRequireEntranceManager;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.Domain.Payments;

namespace ECondo.Application.Commands.Payment.CreateBill;

public sealed record CreateBillCommand(
    Guid EntranceId,
    string Title,
    string? Description,
    decimal Amount,
    bool IsRecurring,
    RecurringInterval? RecurringInterval,
    DateTimeOffset StartDate,
    DateTimeOffset? EndDate)
    : ICommand<Guid>, ICanUpdate<Entrance>
{
    Guid? IResourcePolicy.ResourceId => EntranceId;
}
    
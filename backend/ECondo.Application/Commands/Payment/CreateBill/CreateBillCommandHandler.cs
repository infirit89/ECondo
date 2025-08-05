using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Payments;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Payment.CreateBill;

internal sealed class CreateBillCommandHandler
    (IApplicationDbContext dbContext, IUserContext userContext)
    : ICommandHandler<CreateBillCommand, Guid>
{
    public async Task<Result<Guid, Error>> Handle(CreateBillCommand request, CancellationToken cancellationToken)
    {
        var entrance = await dbContext
            .Entrances
            .FirstAsync(e => 
                e.Id == request.EntranceId,
                cancellationToken: cancellationToken);

        var bill = new Bill
        {
            EntranceId = entrance.Id,
            Title = request.Title,
            Description = request.Description,
            Amount = request.Amount,
            IsRecurring = request.IsRecurring,
            RecurringInterval = request.RecurringInterval,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedByUserId = userContext.UserId,
            CreatedAt = DateTimeOffset.UtcNow,
        };

        await dbContext.Bills.AddAsync(bill, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        if (!bill.IsRecurring)
            await GeneratePaymentsForOneTimeBill(bill, cancellationToken);
        
        return Result<Guid, Error>.Ok(bill.Id);
    }
    
    private async Task GeneratePaymentsForOneTimeBill(Bill bill, CancellationToken cancellationToken)
    {
        var properties = await dbContext.Properties
            .Where(p => p.EntranceId == bill.EntranceId)
            .ToListAsync(cancellationToken);

        if (properties.Count == 0)
            return;

        var splitAmount = bill.Amount / properties.Count;

        foreach (var property in properties)
        {
            var payment = new Domain.Payments.Payment
            {
                BillId = bill.Id,
                PropertyId = property.Id,
                AmountPaid = splitAmount,
                PaidByUserId = null,
                PaymentDate = DateTimeOffset.UtcNow.Date,
                PaymentMethod = "Pending",
            };

            dbContext.Payments.Add(payment);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
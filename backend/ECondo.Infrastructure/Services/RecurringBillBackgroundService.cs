using ECondo.Application.Repositories;
using ECondo.Domain.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ECondo.Infrastructure.Services;

internal class RecurringBillBackgroundService : BackgroundService
{
    public RecurringBillBackgroundService(IServiceProvider serviceProvider,
        ILogger<RecurringBillBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // var now = DateTimeOffset.UtcNow;
                // var tomorrow = now.Date.AddDays(1);
                // var delay = tomorrow - now;
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);

                await GeneratePaymentsForDueBills();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RecurringBillBackgroundService error");
            }
        }
    }

    private async Task GeneratePaymentsForDueBills()
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var today = DateTimeOffset.UtcNow.Date;

        var dueBills = await dbContext
            .Bills
            .Where(b => 
                b.IsRecurring && 
                b.StartDate <= today && 
                (b.EndDate == null || b.EndDate >= today))
            .ToListAsync();

        foreach (var bill in dueBills)
        {
            var properties = await dbContext
                .Properties
                .Where(p => p.EntranceId == bill.EntranceId)
                .ToListAsync();

            var splitAmount = bill.Amount / properties.Count;

            foreach (var property in properties)
            {
                var existingPayment = await dbContext
                    .Payments
                    .FirstOrDefaultAsync(p => 
                        p.BillId == bill.Id && 
                        p.PropertyId == property.Id && 
                        p.PaymentDate.Date == today);

                if (existingPayment != null)
                    continue;

                var payment = new Payment
                {
                    BillId = bill.Id,
                    PropertyId = property.Id,
                    PaidByUserId = null,
                    AmountPaid = splitAmount,
                    PaymentDate = today,
                    PaymentMethod = "Pending"
                };

                dbContext.Payments.Add(payment);
            }
        }
        
        await dbContext.SaveChangesAsync();
    }

    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RecurringBillBackgroundService> _logger;
}
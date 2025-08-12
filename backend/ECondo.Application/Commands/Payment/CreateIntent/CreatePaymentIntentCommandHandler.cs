using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Payments;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Payment.CreateIntent;

internal sealed class CreatePaymentIntentCommandHandler
    (IApplicationDbContext dbContext, IUserContext userContext, IStripeService stripeService)
    : ICommandHandler<CreatePaymentIntentCommand, string>
{
    public async Task<Result<string, Error>> Handle(
        CreatePaymentIntentCommand request, 
        CancellationToken cancellationToken)
    {
        var payment = await dbContext
            .Payments
            .Include(p => p.Bill)
            .Include(p => p.Property)
            .ThenInclude(p => p.Entrance)
            .AsNoTracking()
            .Where(p =>
                p.Id == request.PaymentId &&
                p.Property.PropertyOccupants.Any(po => po.UserId == userContext.UserId))
            .Select(p => new
            {
                p.AmountPaid,
                p.Property.Entrance.StripeAccountId,
            })
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if(payment is null)
            return Result<string, Error>.Fail(PaymentErrors.NotFound(request.PaymentId));
        
        if(string.IsNullOrEmpty(payment.StripeAccountId))
            return Result<string, Error>.Fail(PaymentErrors.InvalidStripeAccount());

        var clientSecret = await stripeService.CreatePaymentIntent(
            payment.AmountPaid, 
            payment.StripeAccountId);
        
        return Result<string, Error>.Ok(clientSecret);
    }
}
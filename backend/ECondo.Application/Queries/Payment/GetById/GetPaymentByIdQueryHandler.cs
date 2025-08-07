using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Payments;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Payment.GetById;

internal sealed class GetPaymentByIdQueryHandler
    (IApplicationDbContext dbContext, IUserContext userContext)
    : IQueryHandler<GetPaymentByIdQuery, PaymentResult>
{
    public async Task<Result<PaymentResult, Error>> Handle(
        GetPaymentByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var payment = await dbContext
            .Payments
            .Include(p => p.Bill)
            .AsNoTracking()
            .Where(p =>
                p.Id == request.PaymentId &&
                p.Property.PropertyOccupants.Any(po => po.UserId == userContext.UserId))
            .Select(p => 
                new PaymentResult(
                    p.Id,
                    p.AmountPaid, 
                    p.Bill.Title,
                    p.PaymentMethod))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if(payment is null)
            return Result<PaymentResult, Error>.Fail(
                PaymentErrors.NotFound(request.PaymentId));
        
        return Result<PaymentResult, Error>.Ok(payment);
    }
}
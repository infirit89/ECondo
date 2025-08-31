using ECondo.Application.Extensions;
using ECondo.Application.Queries.Payment.GetById;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.SharedKernel.Collections;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Payment.GetForProperty;

internal sealed class GetPaymentForPropertyQueryHandler
    (IApplicationDbContext dbContext, IUserContext userContext)
    : IQueryHandler<GetPaymentForPropertyQuery, PagedList<PaymentResult>>
{
    public async Task<Result<PagedList<PaymentResult>, Error>> Handle(GetPaymentForPropertyQuery request, CancellationToken cancellationToken)
    {
        var payments = await dbContext
            .Payments
            .Include(p => p.Bill)
            .AsNoTracking()
            .Where(p =>
                p.PropertyId == request.PropertyId &&
                p.Property.PropertyOccupants.Any(po => po.UserId == userContext.UserId))
            .Select(p =>
                new PaymentResult(
                    p.Id,
                    p.AmountPaid,
                    p.Bill.Title,
                    p.PaymentMethod))
            .ToPagedListAsync(request.Page, request.PageSize, 
                cancellationToken: cancellationToken);
        
        return Result<PagedList<PaymentResult>, Error>.Ok(payments);
    }
}
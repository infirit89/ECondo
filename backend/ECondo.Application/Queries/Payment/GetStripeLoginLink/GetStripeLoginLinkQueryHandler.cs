using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Payments;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Payment.GetStripeLoginLink;

internal sealed class GetStripeLoginLinkQueryHandler
    (IApplicationDbContext dbContext, IStripeService stripeService)
    : IQueryHandler<GetEntranceStripeLoginLinkQuery, string>
{
    public async Task<Result<string, Error>> Handle(GetEntranceStripeLoginLinkQuery request, CancellationToken cancellationToken)
    {
        var entrance = await dbContext
            .Entrances
            .FirstAsync(e => 
                    e.Id == request.EntranceId, 
                cancellationToken: cancellationToken);
        
        if(entrance.StripeAccountId is null)
            return Result<string, Error>.Fail(PaymentErrors.InvalidStripeAccount());

        var link = await stripeService.GetLoginLinkForAccount(entrance.StripeAccountId);
        return Result<string, Error>.Ok(link);
    }
}
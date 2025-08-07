using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Payment.CheckStripeStatus;

internal sealed class CheckEntranceStripeStatusQueryHandler
    (IApplicationDbContext dbContext, IStripeService stripeService)
    : IQueryHandler<CheckEntranceStripeStatusQuery, StripeStatus>
{
    public async Task<Result<StripeStatus, Error>> Handle(
        CheckEntranceStripeStatusQuery request, 
        CancellationToken cancellationToken)
    {
        var entrance = await dbContext
            .Entrances
            .FirstAsync(e => 
                e.Id == request.EntranceId,
                cancellationToken: cancellationToken);
        
        if(entrance.StripeAccountId is null)
            return Result<StripeStatus, Error>.Ok(
                new StripeStatus(false, false));

        var status = await stripeService.GetEntranceAccountStatus(entrance.StripeAccountId);
        return Result<StripeStatus, Error>.Ok(status);
    }
}
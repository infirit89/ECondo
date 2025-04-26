using ECondo.Application.Policies;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Application.Shared;
using ECondo.Domain.Buildings;
using ECondo.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Behaviours;

internal sealed class AddOccupantAuthorizationPipelineBehaviour
    <TRequest, TResponse>
    (IUserContext userContext, IApplicationDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICanAddOccupant
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var canAdd = await dbContext
            .Properties
            .AsNoTracking()
            .Where(p =>
                p.Id == request.PropertyId &&
                p.Entrance.ManagerId == userContext.UserId)
            .AnyAsync(cancellationToken: cancellationToken);

        if (canAdd)
            return await next();
        
        if (Utils.IsTypeResultType<TResponse>())
        {
            var res = Utils.InvokeResultFail<TResponse>(
                [PropertyErrors.Forbidden(request.PropertyId)]);

            if (res is not null)
                return res;
        }

        throw new ForbiddenException();
    }
}
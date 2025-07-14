using ECondo.Application.Extensions;
using ECondo.Application.Policies;
using ECondo.Application.Policies.Property;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Application.Shared;
using ECondo.Domain.Buildings;
using ECondo.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Behaviours;

internal sealed class AccessPropertyAuthorizationPipelineBehaviour
    <TRequest, TResponse>
    (IUserContext userContext, IApplicationDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICanSeeProperty
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext
            .UserRoles
            .IsAdminAsync(userContext.UserId, cancellationToken: cancellationToken);
        
        var canSee = await dbContext
            .Properties
            .AsNoTracking()
            .Where(p =>
                (p.Id == request.PropertyId &&
                (p.Entrance.ManagerId == userContext.UserId ||
                p.PropertyOccupants.Any(po => po.UserId == userContext.UserId))) ||
                isAdmin)
            .AnyAsync(cancellationToken: cancellationToken);

        if (canSee)
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
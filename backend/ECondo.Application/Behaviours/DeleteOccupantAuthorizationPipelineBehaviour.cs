using ECondo.Application.Extensions;
using ECondo.Application.Policies;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Application.Shared;
using ECondo.Domain.Buildings;
using ECondo.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Behaviours;

internal sealed class DeleteOccupantAuthorizationPipelineBehaviour
    <TRequest, TResponse>
    (IUserContext userContext, IApplicationDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICanDeleteOccupant
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext
            .UserRoles
            .IsAdminAsync(userContext.UserId, cancellationToken: cancellationToken);
        
        var canDelete = await dbContext
            .PropertyOccupants
            .AsNoTracking()
            .Where(p =>
                (p.Id == request.OccupantId &&
                (p.Property.Entrance.ManagerId == userContext.UserId ||
                 (p.Property.PropertyOccupants.Any(po =>
                      po.UserId == userContext.UserId &&
                      po.OccupantType.Name == "Собственик") &&
                  p.OccupantType.Name != "Собственик"))) ||
                isAdmin)
            .AnyAsync(cancellationToken: cancellationToken);

        if (canDelete)
            return await next();
        
        if (Utils.IsTypeResultType<TResponse>())
        {
            var res = Utils.InvokeResultFail<TResponse>(
                [PropertyOccupantError.Forbidden(request.OccupantId)]);

            if (res is not null)
                return res;
        }

        throw new ForbiddenException();
    }
}
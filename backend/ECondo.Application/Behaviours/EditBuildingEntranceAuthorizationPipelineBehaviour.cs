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

internal sealed class EditBuildingEntranceAuthorizationPipelineBehaviour
    <TRequest, TResponse>
    (IUserContext userContext, IApplicationDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICanEditEntrance
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext
            .UserRoles
            .IsAdminAsync(userContext.UserId, 
                cancellationToken: cancellationToken);
        
        var canDelete = await dbContext
            .Entrances
            .AsNoTracking()
            .Where(e =>
                (e.BuildingId == request.BuildingId &&
                 e.Number == request.EntranceNumber &&
                 e.ManagerId == userContext.UserId) ||
                isAdmin)
            .AnyAsync(cancellationToken: cancellationToken);

        if (canDelete)
            return await next();
        
        if (Utils.IsTypeResultType<TResponse>())
        {
            var res = Utils.InvokeResultFail<TResponse>(
                [EntranceErrors.Forbidden(request.BuildingId, request.EntranceNumber)]);

            if (res is not null)
                return res;
        }

        throw new ForbiddenException();
    }
}
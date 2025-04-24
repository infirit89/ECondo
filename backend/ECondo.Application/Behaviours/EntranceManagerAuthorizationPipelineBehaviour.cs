using ECondo.Application.Policies;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Application.Shared;
using ECondo.Domain.Exceptions;
using ECondo.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Behaviours;

internal sealed class EntranceManagerAuthorizationPipelineBehaviour
    <TRequest, TResponse>
    (IUserContext userContext, IApplicationDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequireEntranceManager

{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var isManager = await dbContext
            .Entrances
            .AsNoTracking()
            .AnyAsync(e =>
                e.BuildingId == request.BuildingId &&
                e.Number == request.EntranceNumber &&
                e.ManagerId == userContext.UserId,
                cancellationToken: cancellationToken);

        if (isManager)
            return await next();

        if (Utils.IsTypeResultType<TResponse>())
        {
            var res = Utils.InvokeResultFail<TResponse>(
                [CreateForbiddenError()]);

            if (res is not null)
                return res;
        }

        throw new ForbiddenException();
    }

    private static Error CreateForbiddenError() =>
        Error.Forbidden("Entrance.Forbidden", "The User can not access the entrance");
}
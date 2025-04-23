using ECondo.Application.Policies;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
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

        if (typeof(TResponse).IsGenericType &&
            typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<,>))
        {
            Type resultType = typeof(TResponse).GetGenericArguments()[0];
            var failMethodInfo = typeof(Result<,>)
                .MakeGenericType(resultType, typeof(Error))
                .GetMethod(nameof(Result<object, Error>.Fail));

            object? res = failMethodInfo?.Invoke(
                null,
                [CreateForbiddenError()]);

            if (res is not null)
                return (TResponse)res;

        }

        throw new UnauthorizedException();
    }

    private static Error CreateForbiddenError() =>
        Error.Forbidden("Entrance.Forbidden", "The User can not access the entrance");
}
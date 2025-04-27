using ECondo.Application.Policies;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Application.Shared;
using ECondo.Domain.Exceptions;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Behaviours;

internal sealed class AdminAuthorizationPipelineBehaviour
    <TRequest, TResponse>
    (IUserContext userContext, IApplicationDbContext dbContext)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IIsAdmin
{
    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        var isAdmin = await dbContext
            .UserRoles
            .Where(ur =>
                ur.UserId == userContext.UserId &&
                ur.Role.Name == "admin")
            .AnyAsync(cancellationToken: cancellationToken);

        if (isAdmin)
            return await next();
        
        if (Utils.IsTypeResultType<TResponse>())
        {
            var res = Utils.InvokeResultFail<TResponse>(
                [UserErrors.Forbidden(userContext.UserId)]);

            if (res is not null)
                return res;
        }

        throw new ForbiddenException();
    }
}
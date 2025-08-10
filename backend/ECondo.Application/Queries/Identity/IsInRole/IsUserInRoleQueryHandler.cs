using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Identity.IsInRole;

internal sealed class IsUserInRoleQueryHandler
    (IUserContext userContext, IApplicationDbContext dbContext)
    : IQueryHandler<IsUserInRoleQuery>
{
    public async Task<Result<EmptySuccess, Error>> Handle(IsUserInRoleQuery request, CancellationToken cancellationToken)
    {
        var isInRole = await dbContext
            .UserRoles
            .Where(ur =>
                ur.UserId == userContext.UserId &&
                ur.Role.NormalizedName == request.RoleName.ToUpper())
            .AnyAsync(cancellationToken: cancellationToken);
        
        if(!isInRole)
            return Result<EmptySuccess, Error>.Fail(
                RoleErrors.NotFound(userContext.UserId, request.RoleName));

        return Result<EmptySuccess, Error>.Ok();
    }
}
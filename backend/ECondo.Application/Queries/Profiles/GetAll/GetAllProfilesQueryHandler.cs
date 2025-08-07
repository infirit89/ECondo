using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Collections;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Profiles.GetAll;

internal sealed class GetAllProfilesQueryHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetAllProfilesQuery, PagedList<UserProfileResult>>
{
    public async Task<Result<PagedList<UserProfileResult>, Error>> Handle(
        GetAllProfilesQuery request, 
        CancellationToken cancellationToken)
    {
        var profiles = await dbContext
            .UserDetails
            .Include(ud => ud.User)
            .AsNoTracking()
            .Where(ud => ud.User.UserRoles.All(ur => ur.Role.Name != Role.Admin))
            .Select(ud =>
                new UserProfileResult(
                    ud.FirstName,
                    ud.MiddleName,
                    ud.LastName,
                    ud.User.Email!))
            .ToPagedListAsync(request.Page, request.PageSize, 
                cancellationToken: cancellationToken);
        
        return Result<PagedList<UserProfileResult>, Error>.Ok(profiles);
    }
}
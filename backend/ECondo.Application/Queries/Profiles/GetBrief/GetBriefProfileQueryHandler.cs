using ECondo.Application.Data;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Profiles.GetBrief;
internal sealed class GetBriefProfileQueryHandler(
    IApplicationDbContext dbContext,
    IUserContext userContext) 
    : IQueryHandler<
        GetBriefProfileQuery, 
        BriefProfileResult>
{
    public async Task<Result<BriefProfileResult, Error>> 
        Handle(
            GetBriefProfileQuery request,
            CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<BriefProfileResult, Error>
                .Fail(UserErrors.InvalidUser());

        var user = await dbContext
            .Users
            .AsNoTracking()
            .Select(u => new
            {
                u.Id,
                u.UserName,
            })
            .FirstOrDefaultAsync(u => 
                u.Id == userContext.UserId,
                cancellationToken: cancellationToken);

        if(user is null)
            return Result<BriefProfileResult, Error>
                .Fail(UserErrors.InvalidUser());

        var result = await dbContext
            .UserDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(pd => 
                pd.UserId == user.Id,
                cancellationToken: cancellationToken);

        if(result is null)
            return Result<BriefProfileResult, Error>
                .Fail(ProfileErrors.InvalidProfile(user.UserName!));

        return Result<BriefProfileResult, Error>
            .Ok(new BriefProfileResult(
                result.FirstName,
                result.LastName,
                user.UserName!));
    }
}

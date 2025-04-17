using ECondo.Application.Data;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Profiles.GetForUser;

internal sealed class GetProfileQueryHandler(
    IUserContext userContext,
    IApplicationDbContext dbContext) 
    : IQueryHandler<GetProfileQuery, ProfileResult>
{
    public async Task<Result<ProfileResult, Error>> 
        Handle(
            GetProfileQuery request,
            CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<ProfileResult, Error>
                .Fail(UserErrors.InvalidUser());

        var profileDetails =
            await dbContext
                .UserDetails
                .AsNoTracking()
                .FirstOrDefaultAsync(pd => 
                    pd.UserId == userContext.UserId,
                    cancellationToken: cancellationToken);

        if(profileDetails is null)
            return Result<ProfileResult, Error>
                .Fail(ProfileErrors
                    .InvalidProfile((Guid)userContext.UserId));

        return Result<ProfileResult, Error>.Ok(
            new ProfileResult(
                profileDetails.FirstName, 
                profileDetails.MiddleName,
                profileDetails.LastName));
    }
}
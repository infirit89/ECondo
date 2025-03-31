using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;

namespace ECondo.Application.Queries.Profile;

internal sealed class GetProfileQueryHandler(
    IUserContext userContext,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<GetProfileQuery, Result<ProfileResult, Error>>
{
    public async Task<Result<ProfileResult, Error>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<ProfileResult, Error>.Fail(UserErrors.InvalidUser());

        var profileDetails =
            await unitOfWork
                .ProfileDetails
                .FirstOrDefaultAsync(pd => pd.UserId == userContext.UserId);

        if(profileDetails is null)
            return Result<ProfileResult, Error>
                .Fail(ProfileErrors.InvalidProfile((Guid)userContext.UserId));

        return Result<ProfileResult, Error>.Ok(new ProfileResult(
            profileDetails.FirstName, 
            profileDetails.MiddleName,
            profileDetails.LastName));
    }
}
using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Queries.Profile;

internal sealed class GetProfileQueryHandler(UserManager<User> userManager, IUnitOfWork unitOfWork) 
    : IRequestHandler<GetProfileQuery, Result<ProfileResult, Error>>
{
    public async Task<Result<ProfileResult, Error>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(request.Email);
        if(user is null)
            return Result<ProfileResult, Error>.Fail(UserErrors.InvalidUser(request.Email));

        ProfileDetails? profileDetails =
            (await unitOfWork.ProfileDetailsRepository.GetAsync(pd => pd.UserId == user.Id)).FirstOrDefault();

        if(profileDetails is null)
            return Result<ProfileResult, Error>.Fail(ProfileErrors.InvalidProfile(request.Email));

        return Result<ProfileResult, Error>.Ok(new(
            profileDetails.FirstName, 
            profileDetails.MiddleName,
            profileDetails.LastName));
    }
}
using ECondo.Application.Data;
using ECondo.Application.Extensions;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Queries.Profile;
internal sealed class GetBriefProfileQueryHandler(
    UserManager<User> userManager,
    IUnitOfWork unitOfWork) : IRequestHandler<GetBriefProfileQuery, Result<BriefProfileResult, Error>>
{
    public async Task<Result<BriefProfileResult, Error>> Handle(GetBriefProfileQuery request, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindUserByEmailOrNameAsync(request.Username);

        if(user is null)
            return Result<BriefProfileResult, Error>.Fail(UserErrors.InvalidUser(request.Username));

        var result = (await unitOfWork.ProfileDetailsRepository.GetAsync(pd => pd.UserId == user.Id)).FirstOrDefault();
        if(result is null)
            return Result<BriefProfileResult, Error>.Fail(ProfileErrors.InvalidProfile(request.Username));

        return Result<BriefProfileResult, Error>.Ok(new BriefProfileResult(result.FirstName, result.LastName, request.Username));
    }
}
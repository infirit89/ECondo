using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Queries.Profile;
internal sealed class GetBriefProfileQueryHandler(
    UserManager<User> userManager,
    IUnitOfWork unitOfWork,
    IUserContext userContext) : IRequestHandler<GetBriefProfileQuery, Result<BriefProfileResult, Error>>
{
    public async Task<Result<BriefProfileResult, Error>> Handle(GetBriefProfileQuery request, CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<BriefProfileResult, Error>.Fail(UserErrors.InvalidUser());

        User? user = await userManager.FindByIdAsync(userContext.UserId.ToString()!);

        if(user is null)
            return Result<BriefProfileResult, Error>.Fail(UserErrors.InvalidUser());

        var result = await unitOfWork
            .ProfileDetails
            .FirstOrDefaultAsync(pd => pd.UserId == user.Id);

        if(result is null)
            return Result<BriefProfileResult, Error>.Fail(ProfileErrors.InvalidProfile(user.UserName!));

        return Result<BriefProfileResult, Error>
            .Ok(new BriefProfileResult(
                result.FirstName,
                result.LastName,
                user.UserName!));
    }
}
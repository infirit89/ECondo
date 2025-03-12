using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Profile;

internal sealed class UpdateProfileCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork) : IRequestHandler<UpdateProfileCommand, Result<EmptySuccess, Error>>
{
    public async Task<Result<EmptySuccess, Error>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(request.Email);
        if(user is null)
            return Result<EmptySuccess, Error>.Fail(UserErrors.InvalidUser(request.Email));

        ProfileDetails? profile = (await unitOfWork.ProfileDetailsRepository.GetAsync(x => x.UserId == user.Id))
            .FirstOrDefault();

        if(profile is null)
            return Result<EmptySuccess, Error>.Fail(ProfileErrors.InvalidProfile(request.Email));

        profile.FirstName = request.FirstName;
        profile.MiddleName = request.MiddleName;
        profile.LastName = request.LastName;
        unitOfWork.ProfileDetailsRepository.Update(profile);
        await unitOfWork.SaveChangesAsync();
        return Result<EmptySuccess, Error>.Ok();
    }
}

using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Profile;
internal sealed class CreateProfileCommandHandler(UserManager<User> userManager, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProfileCommand, Result<EmptySuccess, Error>>
{
    public async Task<Result<EmptySuccess, Error>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        User? user = await userManager.FindByEmailAsync(request.Username)
                     ?? await userManager.FindByNameAsync(request.Username);

        if(user is null)
            return Result<EmptySuccess, Error>.Fail(UserErrors.InvalidUser(request.Username));

        ProfileDetails profileDetails = new()
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            UserId = user.Id,
            User = user,
        };
        await unitOfWork.ProfileDetailsRepository.Insert(profileDetails);
        await unitOfWork.SaveChangesAsync();
        return Result<EmptySuccess, Error>.Ok();
    }
}

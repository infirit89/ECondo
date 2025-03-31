using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;

namespace ECondo.Application.Commands.Profile;

internal sealed class UpdateProfileCommandHandler(
    IUnitOfWork unitOfWork,
    IUserContext userContext) : IRequestHandler<UpdateProfileCommand, Result<EmptySuccess, Error>>
{
    public async Task<Result<EmptySuccess, Error>> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<EmptySuccess, Error>.Fail(UserErrors.InvalidUser());

        ProfileDetails? profile = (await unitOfWork
                .ProfileDetails
                .GetAsync(x => x.UserId == userContext.UserId))
            .FirstOrDefault();

        if(profile is null)
            return Result<EmptySuccess, Error>
                .Fail(ProfileErrors.InvalidProfile((Guid)userContext.UserId));

        profile.FirstName = request.FirstName;
        profile.MiddleName = request.MiddleName;
        profile.LastName = request.LastName;
        unitOfWork.ProfileDetails.Update(profile);
        await unitOfWork.SaveChangesAsync();
        return Result<EmptySuccess, Error>.Ok();
    }
}

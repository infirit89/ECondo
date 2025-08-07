using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Profiles.Update;

internal sealed class UpdateProfileCommandHandler(
    IApplicationDbContext applicationDbContext,
    IUserContext userContext) 
    : ICommandHandler<UpdateProfileCommand>
{
    public async Task<Result<EmptySuccess, Error>> 
        Handle(
            UpdateProfileCommand request,
            CancellationToken cancellationToken)
    {
        ProfileDetails? profile = await applicationDbContext
            .UserDetails
            .FirstOrDefaultAsync(x => 
                x.UserId == userContext.UserId, 
                cancellationToken: cancellationToken);

        if(profile is null)
            return Result<EmptySuccess, Error>
                .Fail(ProfileErrors
                    .InvalidProfile(userContext.UserId));

        profile.FirstName = request.FirstName;
        profile.MiddleName = request.MiddleName;
        profile.LastName = request.LastName;
        applicationDbContext.UserDetails.Update(profile);
        await applicationDbContext
            .SaveChangesAsync(cancellationToken);
        return Result<EmptySuccess, Error>.Ok();
    }
}

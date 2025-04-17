using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Profiles.Create;
internal sealed class CreateProfileCommandHandler(
    UserManager<User> userManager,
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateProfileCommand>
{
    public async Task<Result<EmptySuccess, Error>> 
        Handle(
            CreateProfileCommand request,
            CancellationToken cancellationToken)
    {
        var user = await userManager
            .FindUserByEmailOrNameAsync(request.Username);

        if(user is null)
            return Result<EmptySuccess, Error>
                .Fail(UserErrors.InvalidUser(request.Username));

        ProfileDetails profileDetails = new()
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            UserId = user.Id,
            User = user,
        };
        await dbContext.UserDetails.AddAsync(
            profileDetails,
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        var phoneResult = await userManager
            .SetPhoneNumberAsync(user, request.PhoneNumber);

        if (phoneResult.Succeeded) 
            return Result<EmptySuccess, Error>.Ok();

        return Result<EmptySuccess, Error>
            .Fail(phoneResult.Errors.ToValidationError());
    }
}

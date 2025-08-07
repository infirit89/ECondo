using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Profiles;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Profiles.Create;
internal sealed class CreateProfileCommandHandler(
    IUserContext userContext,
    IApplicationDbContext dbContext)
    : ICommandHandler<CreateProfileCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
            CreateProfileCommand request,
            CancellationToken cancellationToken)
    {
        var user = await dbContext
            .Users
            .FirstOrDefaultAsync(u => 
                u.Id == userContext.UserId,
                cancellationToken: cancellationToken);

        if(user is null)
            return Result<EmptySuccess, Error>
                .Fail(UserErrors
                    .InvalidUser(userContext.UserId));

        ProfileDetails profileDetails = new()
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            UserId = user.Id,
        };
        await dbContext.UserDetails.AddAsync(
            profileDetails,
            cancellationToken);

        user.PhoneNumber = request.PhoneNumber;
        dbContext.Users.Update(user);
        
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<EmptySuccess, Error>.Ok();
    }
}

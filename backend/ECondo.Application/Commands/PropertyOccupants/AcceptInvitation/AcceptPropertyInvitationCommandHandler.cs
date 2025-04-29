using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.PropertyOccupants.AcceptInvitation;

internal sealed class AcceptPropertyInvitationCommandHandler
    (IApplicationDbContext dbContext, IUserContext userContext)
    : ICommandHandler<AcceptPropertyInvitationCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(AcceptPropertyInvitationCommand request, CancellationToken cancellationToken)
    {
        User user = await dbContext
            .Users
            .FirstAsync(u => u.Id == userContext.UserId,
                cancellationToken: cancellationToken);

        if(user.Email != request.Email)
            return Result<EmptySuccess, Error>.Fail(
                UserErrors.InvalidUser(request.Email));

        var propertyOccupant = await dbContext
            .PropertyOccupants
            .FirstOrDefaultAsync(po =>
                po.InvitationToken == request.Token &&
                po.Email == request.Email,
                cancellationToken: cancellationToken);

        if(propertyOccupant is null)
            return Result<EmptySuccess, Error>.Fail(
                PropertyOccupantError.NotFound(request.Email));

        if(propertyOccupant.IsInvitationExpired())
            return Result<EmptySuccess, Error>.Fail(
                PropertyOccupantError.InvitationExpired());

        propertyOccupant.UserId = user.Id;
        propertyOccupant.InvitationToken = null;
        propertyOccupant.InvitationExpiresAt = null;
        propertyOccupant.InvitationSentAt = null;
        propertyOccupant.InvitationStatus = InvitationStatus.Accepted;

        dbContext.PropertyOccupants.Update(propertyOccupant);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<EmptySuccess, Error>.Ok();
    }
}
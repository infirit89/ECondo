using ECondo.Application.Events.PropertyOccupant;
using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.PropertyOccupants.Update;

internal sealed class UpdatePropertyOccupantCommandHandler
    (IApplicationDbContext dbContext, IPublisher publisher)
    : ICommandHandler<UpdatePropertyOccupantCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        UpdatePropertyOccupantCommand request, 
        CancellationToken cancellationToken)
    {
        var occupantType = await dbContext
            .OccupantTypes
            .FirstOrDefaultAsync(ot => ot.Name == request.Type, 
                cancellationToken: cancellationToken);
        
        if(occupantType is null)
            return Result<EmptySuccess, Error>.Fail(
                OccupantTypeErrors.Invalid(request.Type));
        
        var occupant = await dbContext
            .PropertyOccupants
            .FirstAsync(po => po.Id == request.OccupantId, 
                cancellationToken: cancellationToken);

        occupant.FirstName = request.FirstName;
        occupant.MiddleName = request.MiddleName;
        occupant.LastName = request.LastName;
        occupant.OccupantTypeId = occupantType.Id;

        if (occupant.Email != request.Email)
        {
            if (!string.IsNullOrEmpty(request.Email))
            {
                occupant.UserId = null;
                occupant.InvitationToken = Guid.NewGuid();
                occupant.InvitationExpiresAt = DateTimeOffset.UtcNow.AddHours(48);
                occupant.InvitationSentAt = DateTimeOffset.UtcNow;
                occupant.InvitationStatus = InvitationStatus.Pending;
                
                await publisher.Publish(
                    new OccupantInvitedEvent(
                        occupant.InvitationToken, 
                        request.Email!,
                        occupant.FirstName, 
                        occupant.InvitationExpiresAt.Value,
                        request.ReturnUri), 
                    cancellationToken);
            }
            else
            {
                occupant.UserId = null;
                occupant.InvitationToken = null;
                occupant.InvitationExpiresAt = null;
                occupant.InvitationSentAt = null;
                occupant.InvitationStatus = InvitationStatus.NotInvited;
            }
        }

        occupant.Email = request.Email;
        dbContext.PropertyOccupants.Update(occupant);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<EmptySuccess, Error>.Ok();
    }
}
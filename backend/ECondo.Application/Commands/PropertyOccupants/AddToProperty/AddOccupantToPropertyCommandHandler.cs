using ECondo.Application.Events.PropertyOccupant;
using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.SharedKernel.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.PropertyOccupants.AddToProperty;

internal sealed class AddOccupantToPropertyCommandHandler
    (IApplicationDbContext dbContext, IPublisher publisher)
    : ICommandHandler<AddOccupantToPropertyCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        AddOccupantToPropertyCommand request,
        CancellationToken cancellationToken)
    {
        var property = await dbContext
            .Properties
            .FirstAsync(p =>
                p.Id == request.PropertyId,
                cancellationToken: cancellationToken);

        var occupantType = await dbContext
            .OccupantTypes
            .FirstOrDefaultAsync(ot =>
                ot.Name.ToLower() == request.OccupantType.ToLower(),
                cancellationToken: cancellationToken);
        
        if(occupantType is null)
            return Result<EmptySuccess, Error>.Fail(
                OccupantTypeErrors.Invalid(request.OccupantType));

        if(await IsDuplicateAsync(property.Id, request.Email, occupantType))
            return Result<EmptySuccess, Error>.Fail(
                PropertyOccupantError.Duplicate(request.Email!, occupantType.Name));

        var propertyOccupant = new PropertyOccupant
        {
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Email = request.Email,
            PropertyId = property.Id,
            OccupantTypeId = occupantType.Id,
        };

        if (!string.IsNullOrEmpty(request.Email))
        {
            propertyOccupant.InvitationToken = Guid.NewGuid();
            // todo: add a configuration property
            propertyOccupant.InvitationExpiresAt = DateTimeOffset.UtcNow.AddHours(48);
            propertyOccupant.InvitationSentAt = DateTimeOffset.UtcNow;
            propertyOccupant.InvitationStatus = InvitationStatus.Pending;

            await publisher.Publish(
                new OccupantInvitedEvent(
                    propertyOccupant.InvitationToken, 
                    propertyOccupant.Email!,
                    propertyOccupant.FirstName, 
                    propertyOccupant.InvitationExpiresAt.Value,
                    request.ReturnUri), 
                cancellationToken);
        }

        await dbContext
            .PropertyOccupants
            .AddAsync(
                propertyOccupant,
                cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<EmptySuccess, Error>.Ok();
    }

    // todo: put in a validator doesn't need to be here
    private async Task<bool> IsDuplicateAsync(Guid propertyId, string? email, OccupantType occupantType)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        return await dbContext.PropertyOccupants.AnyAsync(o =>
            o.PropertyId == propertyId &&
            o.Email == email &&
            o.OccupantType == occupantType);
    }
}
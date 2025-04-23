using ECondo.Application.Repositories;
using ECondo.Domain;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Properties.Create;

internal sealed class CreatePropertyCommandHandler
    (IApplicationDbContext dbContext)
    : ICommandHandler<CreatePropertyCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        CreatePropertyCommand request,
        CancellationToken cancellationToken)
    {
        var entrance = await dbContext
            .Entrances
            .FirstOrDefaultAsync(e =>
                e.BuildingId == request.BuildingId &&
                e.Number == request.EntranceNumber,
                cancellationToken: cancellationToken);

        if (entrance is null)
            return Result<EmptySuccess, Error>.Fail(
                EntranceErrors.InvalidEntrance(
                    request.BuildingId,
                    request.EntranceNumber));

        var propertyType = await dbContext
            .PropertyTypes
            .FirstOrDefaultAsync(pt => 
                pt.Name == request.PropertyType,
                cancellationToken: cancellationToken);

        if(propertyType is null)
            return Result<EmptySuccess, Error>.Fail(
                PropertyTypeErrors
                    .InvalidPropertyType(request.PropertyType));

        Property? property = await dbContext
            .Properties
            .FirstOrDefaultAsync(p =>
                p.EntranceId == entrance.Id &&
                p.Number == request.Number,
                cancellationToken: cancellationToken);

        if(property is not null)
            return Result<EmptySuccess, Error>.Fail(
                PropertyErrors.AlreadyExists(
                    request.Number, entrance.Id));

        property = new Property()
        {
            Floor = request.Floor,
            Number = request.Number,
            PropertyTypeId = propertyType.Id,
            BuiltArea = request.BuiltArea,
            IdealParts = request.IdealParts,
            EntranceId = entrance.Id
        };

        await dbContext
            .Properties
            .AddAsync(property, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<EmptySuccess, Error>.Ok();
    }
}
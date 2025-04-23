using ECondo.Application.Repositories;
using ECondo.Domain;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Properties.Update;

internal sealed class UpdatePropertyCommandHandler
    (IApplicationDbContext dbContext)
    : ICommandHandler<UpdatePropertyCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        UpdatePropertyCommand request,
        CancellationToken cancellationToken)
    {
        var propertyType = await dbContext
            .PropertyTypes
            .FirstOrDefaultAsync(pt => 
                pt.Name == request.PropertyType,
                cancellationToken: cancellationToken);

        if(propertyType is null)
            return Result<EmptySuccess, Error>.Fail(
                PropertyTypeErrors
                    .InvalidPropertyType(request.PropertyType));

        var property = await dbContext
            .Properties
            .FirstOrDefaultAsync(p => 
                p.Id == request.PropertyId,
                cancellationToken: cancellationToken);

        if(property is null)
            return Result<EmptySuccess, Error>.Fail(
                PropertyErrors.InvalidProperty(request.PropertyId));

        property.Floor = request.Floor;
        property.Number = request.Number;
        property.PropertyTypeId = propertyType.Id;
        property.BuiltArea = request.BuiltArea;
        property.IdealParts = request.IdealParts;

        dbContext.Properties.Update(property);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<EmptySuccess, Error>.Ok();
    }
}
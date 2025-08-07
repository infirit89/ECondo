using ECondo.Application.Repositories;
using ECondo.Domain;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
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

        var propertyToUpdate = await dbContext
            .Properties
            .FirstAsync(p => 
                p.Id == request.PropertyId,
                cancellationToken: cancellationToken);
        
        var property = await dbContext
            .Properties
            .FirstOrDefaultAsync(p =>
                p.Number == request.Number &&
                p.EntranceId == propertyToUpdate.EntranceId, 
                cancellationToken: cancellationToken);
        
        if(property is not null && property.Id != propertyToUpdate.Id)
            return Result<EmptySuccess, Error>.Fail(
                PropertyErrors.AlreadyExists(
                    property.Number, property.EntranceId));
        
        propertyToUpdate.Floor = request.Floor;
        propertyToUpdate.Number = request.Number;
        propertyToUpdate.PropertyTypeId = propertyType.Id;
        propertyToUpdate.BuiltArea = request.BuiltArea;
        propertyToUpdate.IdealParts = request.IdealParts;

        dbContext.Properties.Update(propertyToUpdate);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<EmptySuccess, Error>.Ok();
    }
}
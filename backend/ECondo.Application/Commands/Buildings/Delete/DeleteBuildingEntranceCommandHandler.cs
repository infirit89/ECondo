using ECondo.Application.Repositories;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Buildings.Delete;

internal sealed class DeleteBuildingEntranceCommandHandler
    (IApplicationDbContext dbContext)
    : ICommandHandler<DeleteBuildingEntranceCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        DeleteBuildingEntranceCommand request, 
        CancellationToken cancellationToken)
    {
        var entrance = await dbContext
            .Entrances
            .Include(e => e.Properties)
            .ThenInclude(e => e.PropertyOccupants)
            .FirstAsync(e => 
                e.Id == request.EntranceId, 
                cancellationToken: cancellationToken);

        foreach (var property in entrance.Properties)
            dbContext.PropertyOccupants.RemoveRange(property.PropertyOccupants);
        
        dbContext.Properties.RemoveRange(entrance.Properties);
        dbContext.Entrances.Remove(entrance);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<EmptySuccess, Error>.Ok();
    }
}
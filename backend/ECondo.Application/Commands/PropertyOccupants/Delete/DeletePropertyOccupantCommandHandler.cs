using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.PropertyOccupants.Delete;

internal sealed class DeletePropertyOccupantCommandHandler
    (IApplicationDbContext dbContext)
    : ICommandHandler<DeletePropertyOccupantCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        DeletePropertyOccupantCommand request, 
        CancellationToken cancellationToken)
    {
        var occupant = await dbContext
            .PropertyOccupants
            .FirstAsync(po => po.Id == request.OccupantId, 
                cancellationToken: cancellationToken);

        dbContext.PropertyOccupants.Remove(occupant);
        await dbContext.SaveChangesAsync(cancellationToken);
        
        return Result<EmptySuccess, Error>.Ok();
    }
}
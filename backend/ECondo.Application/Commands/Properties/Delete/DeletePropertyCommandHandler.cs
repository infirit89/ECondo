using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Properties.Delete;

internal sealed class DeletePropertyCommandHandler
    (IApplicationDbContext dbContext)
    : ICommandHandler<DeletePropertyCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        DeletePropertyCommand request,
        CancellationToken cancellationToken)
    {
        var property = await dbContext
            .Properties
            .Include(p => p.PropertyOccupants)
            .FirstOrDefaultAsync(p => 
                p.Id == request.PropertyId,
                cancellationToken: cancellationToken);

        if(property is null)
            return Result<EmptySuccess, Error>.Fail(
                PropertyErrors.InvalidProperty(request.PropertyId));

        dbContext.PropertyOccupants.RemoveRange(property.PropertyOccupants);
        dbContext.Properties.Remove(property);
        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<EmptySuccess, Error>.Ok();
    }
}
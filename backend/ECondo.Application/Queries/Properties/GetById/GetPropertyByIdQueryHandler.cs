using ECondo.Application.Data;
using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Properties.GetById;

internal sealed class GetPropertyByIdQueryHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetPropertyByIdQuery, PropertyResult>
{
    public async Task<Result<PropertyResult, Error>> Handle(
        GetPropertyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var result = await dbContext
            .Properties
            .AsNoTracking()
            .Select(p =>
                new
                {
                    p.Id,
                    p.Floor,
                    p.Number,
                    PropertyType = p.PropertyType.Name,
                    p.BuiltArea,
                    p.IdealParts
                })
            .FirstAsync(p => 
                p.Id == request.PropertyId,
                cancellationToken: cancellationToken);

        return Result<PropertyResult, Error>.Ok(
            new PropertyResult(
                result.Id,
                result.Floor,
                result.Number,
                result.PropertyType,
                result.BuiltArea,
                result.IdealParts));
    }
}
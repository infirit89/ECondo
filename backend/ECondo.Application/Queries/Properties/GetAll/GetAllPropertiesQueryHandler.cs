using ECondo.Application.Data;
using ECondo.Application.Data.Occupant;
using ECondo.Application.Data.Property;
using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Properties.GetAll;

internal sealed class GetAllPropertiesQueryHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetAllPropertiesQuery, PagedList<PropertyOccupantResult>>
{
    public async Task<Result<PagedList<PropertyOccupantResult>, Error>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
    {
        var propertiesQuery = dbContext
            .Properties
            .Include(p => p.PropertyType)
            .Include(p => p.PropertyOccupants)
            .AsNoTracking();

        if (request.EntranceFilter is not null)
        {
            propertiesQuery = propertiesQuery
                .Where(p =>
                    p.Entrance.Number == request.EntranceFilter.EntranceNumber &&
                    p.Entrance.BuildingId == request.EntranceFilter.BuildingId);
        }

        var properties = await propertiesQuery
            .Select(p =>
                new PropertyOccupantResult(
                    new PropertyResult(p.Id,
                    p.Floor,
                    p.Number,
                    p.PropertyType.Name,
                    p.BuiltArea,
                    p.IdealParts),
                    p.PropertyOccupants.Take(5).Select(po => new BriefOccupantResult(po.FirstName, po.LastName)),
                    Math.Max(p.PropertyOccupants.Count - 5, 0)))
            .ToPagedListAsync(request.Page, request.PageSize, cancellationToken: cancellationToken);
        
        return Result<PagedList<PropertyOccupantResult>, Error>.Ok(properties);
    }
}
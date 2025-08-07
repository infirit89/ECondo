using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.OccupantTypes.GetAll;

internal sealed class GetAllOccupantTypesQueryHandler
    (IApplicationDbContext dbContext)
    : IQueryHandler<GetAllOccupantTypesQuery, OccupantTypeNameResult>
{
    public async Task<Result<OccupantTypeNameResult, Error>> Handle(GetAllOccupantTypesQuery request, CancellationToken cancellationToken)
    {
        var result = await dbContext
            .OccupantTypes
            .AsNoTracking()
            .Select(pt => pt.Name)
            .ToArrayAsync(cancellationToken: cancellationToken);

        return Result<OccupantTypeNameResult, Error>.Ok(
            new OccupantTypeNameResult(result));
    }
}
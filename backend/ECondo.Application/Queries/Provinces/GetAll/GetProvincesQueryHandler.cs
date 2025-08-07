using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.Provinces.GetAll;

internal sealed class GetProvincesQueryHandler(
    IApplicationDbContext dbContext)
    : IQueryHandler<GetProvincesQuery, ProvinceNameResult>
{
    public async Task<Result<ProvinceNameResult, Error>> 
        Handle(
            GetProvincesQuery request,
            CancellationToken cancellationToken)
    {
        var provinces = await dbContext
            .Provinces
            .AsNoTracking()
            .Select(x => x.Name)
            .ToArrayAsync(cancellationToken: cancellationToken);

        return Result<ProvinceNameResult, Error>.Ok(
            new ProvinceNameResult(
                provinces));
    }
}
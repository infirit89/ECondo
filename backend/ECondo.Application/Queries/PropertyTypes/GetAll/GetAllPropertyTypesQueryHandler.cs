using ECondo.Application.Commands;
using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Queries.PropertyTypes.GetAll;

internal sealed class GetAllPropertyTypesQueryHandler
    (IApplicationDbContext dbContext)
    : ICommandHandler<
        GetAllPropertyTypesQuery,
        PropertyTypeNameResult>
{
    public async Task<Result<PropertyTypeNameResult, Error>> Handle(
        GetAllPropertyTypesQuery request,
        CancellationToken cancellationToken)
    {
        var result = await dbContext
            .PropertyTypes
            .AsNoTracking()
            .Select(pt => pt.Name)
            .ToArrayAsync(cancellationToken: cancellationToken);

        return Result<PropertyTypeNameResult, Error>.Ok(
            new PropertyTypeNameResult(result));
    }
}
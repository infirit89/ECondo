using ECondo.Application.Services;
using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Queries.Province;

internal sealed class GetProvincesQueryHandler(
    IUnitOfWork unitOfWork)
    : IRequestHandler<GetProvincesQuery, Result<ProvinceNameResult, Error>>
{
    public async Task<Result<ProvinceNameResult, Error>> 
        Handle(
            GetProvincesQuery request,
            CancellationToken cancellationToken)
    {
        var provinces = (await unitOfWork
            .Provinces
            .GetAsync())
            .Select(x => x.Name);

        return Result<ProvinceNameResult, Error>.Ok(new ProvinceNameResult(
            provinces));
    }
}
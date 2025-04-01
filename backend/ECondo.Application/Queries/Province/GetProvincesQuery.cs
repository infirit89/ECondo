using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Queries.Province;

public sealed record ProvinceNameResult(IEnumerable<string> Provinces);

public sealed record GetProvincesQuery : IRequest<Result<ProvinceNameResult, Error>>;

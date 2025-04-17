namespace ECondo.Application.Queries.Provinces.GetAll;

public sealed record ProvinceNameResult(
    IEnumerable<string> Provinces);

public sealed record GetProvincesQuery 
    : IQuery<ProvinceNameResult>;

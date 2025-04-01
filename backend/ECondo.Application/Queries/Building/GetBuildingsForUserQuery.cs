using ECondo.Application.Data;
using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Queries.Building;

public sealed record GetBuildingsForUserQuery
    : IRequest<Result<BuildingResult[], Error>>;

using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Queries.Building;

public sealed record IsUserInBuildingQuery(Guid BuildingId) : IRequest<Result<EmptySuccess, Error>>;

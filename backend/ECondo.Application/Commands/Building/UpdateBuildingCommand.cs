using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Building;

public sealed record UpdateBuildingCommand(
    Guid BuildingId,
    string BuildingName,
    string ProvinceName,
    string Municipality,
    string SettlementPlace,
    string Neighborhood,
    string PostalCode,
    string Street,
    string StreetNumber,
    string BuildingNumber)
    : IRequest<Result<EmptySuccess, Error>>;

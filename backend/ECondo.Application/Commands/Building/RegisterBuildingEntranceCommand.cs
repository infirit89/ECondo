using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Building;

public sealed record RegisterBuildingEntranceCommand(
	string BuildingName,
	string ProvinceName,
	string Municipality,
	string SettlementPlace,
	string Neighborhood,
	string PostalCode,
	string Street,
	string StreetNumber,
	string BuildingNumber,
	string EntranceNumber) : IRequest<Result<EmptySuccess, Error>>;

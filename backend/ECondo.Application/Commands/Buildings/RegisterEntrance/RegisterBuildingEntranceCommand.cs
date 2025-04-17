namespace ECondo.Application.Commands.Buildings.RegisterEntrance;

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
	string EntranceNumber) 
    : ICommand;

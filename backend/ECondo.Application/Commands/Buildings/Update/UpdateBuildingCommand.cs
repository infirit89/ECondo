namespace ECondo.Application.Commands.Buildings.Update;

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
    : ICommand;

namespace ECondo.Application.Data;

public sealed record BuildingResult(
    Guid Id,
    string Name,
    string ProvinceName,
    string Municipality,
    string SettlementPlace,
    string Neighborhood,
    string PostalCode,
    string Street,
    string StreetNumber,
    string BuildingNumber,
    string EntranceNumber,
    Guid EntranceId);

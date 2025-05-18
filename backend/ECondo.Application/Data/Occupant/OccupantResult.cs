using ECondo.Domain.Buildings;

namespace ECondo.Application.Data.Occupant;

public sealed record OccupantResult(
    Guid Id,
    string FirstName,
    string MiddleName,
    string LastName,
    string Type,
    string? Email,
    InvitationStatus InvitationStatus);
    
using ECondo.Application.Data.Occupant;

namespace ECondo.Application.Data.Property;

public sealed record PropertyOccupantResult(
    PropertyResult Property, 
    IEnumerable<BriefOccupantResult> Occupants, 
    int RemainingOccupants);
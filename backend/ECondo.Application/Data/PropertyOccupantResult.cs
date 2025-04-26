namespace ECondo.Application.Data;

public sealed record PropertyOccupantResult(
    Guid Id,
    string Floor,
    string Number,
    string PropertyType,
    int BuiltArea,
    int IdealParts);
namespace ECondo.Application.Data.Property;

public sealed record PropertyResult(
    Guid Id,
    string Floor,
    string Number,
    string PropertyType,
    int BuiltArea,
    int IdealParts);

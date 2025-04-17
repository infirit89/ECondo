namespace ECondo.Domain.Shared;

public sealed record ValidationError(Error[] Errors) 
    : Error("Validation.General",
    "One or more validation errors occurred",
    ErrorType.Validation)
{
    public Error[] Errors { get; set; } = Errors;
}
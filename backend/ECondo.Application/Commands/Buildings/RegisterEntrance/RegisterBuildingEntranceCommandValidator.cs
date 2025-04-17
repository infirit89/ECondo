using FluentValidation;

namespace ECondo.Application.Commands.Buildings.RegisterEntrance;

internal class RegisterBuildingEntranceCommandValidator
    : AbstractValidator<RegisterBuildingEntranceCommand>
{
    public RegisterBuildingEntranceCommandValidator()
    {
        RuleFor(b => b.BuildingName)
            .NotEmpty();

        RuleFor(b => b.ProvinceName)
            .NotEmpty();

        RuleFor(b => b.Municipality)
            .NotEmpty();

        RuleFor(b => b.SettlementPlace)
            .NotEmpty();

        RuleFor(b => b.Neighborhood)
            .NotEmpty();

        RuleFor(b => b.PostalCode)
            .NotEmpty();

        RuleFor(b => b.Street)
            .NotEmpty();

        RuleFor(b => b.StreetNumber)
            .NotEmpty();

        RuleFor(b => b.BuildingNumber)
            .NotEmpty();

        RuleFor(b => b.EntranceNumber)
            .NotEmpty();
    }
}

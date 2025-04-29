using FluentValidation;

namespace ECondo.Application.Commands.Payment.CreateBill;

internal sealed class CreateBillCommandValidator
    : AbstractValidator<CreateBillCommand>
{
    public CreateBillCommandValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty();

        RuleFor(b => b.StartDate)
            .LessThan(b => b.EndDate)
            .When(b => b.EndDate is not null);

        RuleFor(b => b.RecurringInterval)
            .NotNull()
            .When(b => b.IsRecurring)
            .WithMessage("Recurring interval cannot be null when bill is recurring");
    }
}
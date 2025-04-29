using ECondo.Domain.Shared;

namespace ECondo.Domain.Payments;

public static class PaymentErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound(
            "Payments.NotFound",
            $"The payment with Id = '{id}' was not found");

    public static Error InvalidStripeAccount() =>
        Error.NotFound(
            "Payments.NotFound",
            $"The stripe account for the entrance was not found");
}
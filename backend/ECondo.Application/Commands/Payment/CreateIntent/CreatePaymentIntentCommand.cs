namespace ECondo.Application.Commands.Payment.CreateIntent;

public record CreatePaymentIntentCommand(Guid PaymentId) 
    : ICommand<string>;

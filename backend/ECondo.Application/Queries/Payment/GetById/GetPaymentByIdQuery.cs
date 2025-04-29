namespace ECondo.Application.Queries.Payment.GetById;

public sealed record PaymentResult(Guid Id, decimal AmountPaid, string BillTitle, string Status); 

public sealed record GetPaymentByIdQuery(Guid PaymentId) : IQuery<PaymentResult>;
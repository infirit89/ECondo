using ECondo.Application.Queries.Payment.GetById;
using ECondo.SharedKernel.Collections;

namespace ECondo.Application.Queries.Payment.GetForProperty;

public sealed record GetPaymentForPropertyQuery(Guid PropertyId, int Page, int PageSize) : 
    IQuery<PagedList<PaymentResult>>;

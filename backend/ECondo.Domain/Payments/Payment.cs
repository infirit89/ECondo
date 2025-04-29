using ECondo.Domain.Buildings;
using ECondo.Domain.Users;

namespace ECondo.Domain.Payments;

public sealed class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid BillId { get; set; }
    public Bill Bill { get; set; } = null!;
    
    public Guid PropertyId { get; set; }
    public Property Property { get; set; } = null!;
    
    public Guid? PaidByUserId { get; set; }
    public User? PaidByUser { get; set; }
    
    public decimal AmountPaid { get; set; }
    public DateTimeOffset PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = null!;
}
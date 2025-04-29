using ECondo.Domain.Buildings;
using ECondo.Domain.Users;

namespace ECondo.Domain.Payments;

public sealed class Bill
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    
    public Guid EntranceId { get; set; }
    public Entrance Entrance { get; set; } = null!;
    
    public bool IsRecurring { get; set; }
    public RecurringInterval? RecurringInterval { get; set; }
    
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    
    public Guid CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; } = null!;

    public BillStatus Status { get; set; } = BillStatus.Pending;

    public HashSet<Payment> Payments { get; set; } = [];
}

public enum RecurringInterval
{
    Monthly,
    Quarterly,
    Yearly
}

public enum BillStatus
{
    Pending,
    Paid,
    Cancelled,
}
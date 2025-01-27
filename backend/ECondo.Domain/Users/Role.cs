namespace ECondo.Domain.Users;

public class Role
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = null!;
    public string NormalizedName { get; set; } = null!;
    public Guid ConcurrencyStamp { get; set; } = Guid.NewGuid();
}

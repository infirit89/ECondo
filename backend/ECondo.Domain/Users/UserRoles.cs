namespace ECondo.Domain.Users;

public sealed class UserRole
{
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
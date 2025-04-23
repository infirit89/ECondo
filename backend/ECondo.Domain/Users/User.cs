using ECondo.Domain.Abstractions;
using ECondo.Domain.Buildings;
using ECondo.Domain.Profiles;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Domain.Users;
public class User : IdentityUser<Guid>, ISoftDeletable
{
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    // ---- navigational properties ----
    public HashSet<UserRole> UserRoles { get; set; } = [];
    public HashSet<UserClaim> UserClaims { get; set; } = [];
    public HashSet<UserLogin> UserLogins { get; set; } = [];
    public HashSet<UserToken> UserTokens { get; set; } = [];

    public HashSet<ProfileDetails> UserDetails { get; set; } = [];

    public HashSet<PropertyOccupant> PropertyOccupants { get; set; } = [];

    public HashSet<Entrance> Entrances { get; set; } = [];
    // ---------------------------------

    // ---- soft delete stuffs ----
    public bool IsDeleted { private set; get; }
    public DateTimeOffset? DeletedAt { private set; get; }
    public void Undo()
    {
        IsDeleted = false;
        DeletedAt = null;
    }
    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTimeOffset.UtcNow;
    }
    // ----------------------------
}

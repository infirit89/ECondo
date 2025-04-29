using ECondo.Domain.Buildings;
using ECondo.Domain.Profiles;
using ECondo.Domain.Provinces;
using ECondo.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using ECondo.Application.Repositories;
using ECondo.Domain.Payments;

namespace ECondo.Infrastructure.Contexts;

internal class ECondoDbContext(
    DbContextOptions<ECondoDbContext> options) 
    : IdentityDbContext<
        User,
        Role,
        Guid,
        UserClaim,
        UserRole,
        UserLogin,
        RoleClaim,
        UserToken>(options),
    IApplicationDbContext
{
    // --------------------------- User data ---------------------------
    public override DbSet<User> Users { get; set; } = null!;
    public override DbSet<Role> Roles { get; set; } = null!;
    public override DbSet<UserRole> UserRoles { get; set; } = null!;
    public override DbSet<UserClaim> UserClaims { get; set; } = null!;
    public override DbSet<UserLogin> UserLogins { get; set; } = null!;
    public override DbSet<UserToken> UserTokens { get; set; } = null!;
    public override DbSet<RoleClaim> RoleClaims { get; set; } = null!;

    public DbSet<ProfileDetails> UserDetails { get; set; } = null!;
    // -----------------------------------------------------------------


    public virtual DbSet<Building> Buildings { get; set; } = null!;
    public virtual DbSet<Entrance> Entrances { get; set; } = null!;
    public virtual DbSet<Province> Provinces { get; set; } = null!;
    public virtual DbSet<OccupantType> OccupantTypes { get; set; } = null!;
    public virtual DbSet<Property> Properties { get; set; } = null!;
    public virtual DbSet<PropertyType> PropertyTypes { get; set; } = null!;
    public virtual DbSet<PropertyOccupant> PropertyOccupants { get; set; } = null!;


    public DbSet<Bill> Bills { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Assembly current = Assembly.GetExecutingAssembly();
        modelBuilder.ApplyConfigurationsFromAssembly(current);
    }
}

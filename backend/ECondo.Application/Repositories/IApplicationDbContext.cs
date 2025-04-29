using ECondo.Domain.Buildings;
using ECondo.Domain.Payments;
using ECondo.Domain.Profiles;
using ECondo.Domain.Provinces;
using ECondo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Repositories;

public interface IApplicationDbContext
{
    #region Identity

    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    DbSet<UserRole> UserRoles { get; }
    DbSet<UserClaim> UserClaims { get; }
    DbSet<UserLogin> UserLogins { get; }
    DbSet<UserToken> UserTokens { get; }
    DbSet<RoleClaim> RoleClaims { get; }

    #endregion

    #region Profile

    DbSet<ProfileDetails> UserDetails { get; }

    #endregion

    #region Building
    DbSet<Building> Buildings { get; }
    DbSet<Entrance> Entrances { get; }
    DbSet<Province> Provinces { get; }
    DbSet<OccupantType> OccupantTypes { get; }
    DbSet<Property> Properties { get; }
    DbSet<PropertyType> PropertyTypes { get; }
    DbSet<PropertyOccupant> PropertyOccupants { get; }
    #endregion

    #region Payment

    DbSet<Bill> Bills { get; }
    DbSet<Payment> Payments { get; }

    #endregion
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

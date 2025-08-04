using ECondo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Extensions;

public static class DbSetExtension
{
    public static Task<bool> IsAdminAsync(this DbSet<UserRole> userRoles, Guid userId, CancellationToken cancellationToken = default)
    {
        return userRoles
            .Where(ur => ur.UserId == userId && ur.Role.Name == "admin")
            .AsNoTracking()
            .AnyAsync(cancellationToken: cancellationToken);
    }
}
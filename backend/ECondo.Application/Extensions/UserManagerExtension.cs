using ECondo.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Extensions;
public static class UserManagerExtension
{
    public static async Task<User?> FindUserByEmailOrNameAsync(this UserManager<User> userManager, string name)
    {
        return await userManager.FindByEmailAsync(name)
               ?? await userManager.FindByNameAsync(name);
    }
}

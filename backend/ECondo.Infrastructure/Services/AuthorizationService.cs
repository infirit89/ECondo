using ECondo.Application.Authorization;
using ECondo.Application.Services;
using ECondo.Domain.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace ECondo.Infrastructure.Services;

public sealed class AuthorizationService
    (IServiceProvider serviceProvider)
    : IAuthorizationService
{
    public async Task<bool> CanPerformActionAsync<T>(Guid userId,
        Guid? resourceId, 
        AccessLevel resourceAccess,
        CancellationToken cancellationToken = default) 
        where T : class
    {
        var accessLevel = await GetAccessLevelAsync<T>(userId, 
            resourceId, cancellationToken);

        return (resourceAccess & accessLevel) != 0;
    }

    public Task<AccessLevel> GetAccessLevelAsync<T>(Guid userId, 
        Guid? resourceId, 
        CancellationToken cancellationToken = default) 
        where T : class
    {
        var handler = GetHandler<T>();
        return handler.GetAccessLevelAsync(userId, resourceId, cancellationToken);
    }

    public Task<IQueryable<T>> ApplyDataFilterAsync<T>(
        IQueryable<T> query, 
        Guid userId, 
        CancellationToken cancellationToken = default) 
        where T : class
    {
        var handler = GetHandler<T>();
        return handler.ApplyDataFilterAsync(query, userId, cancellationToken);
    }
    
    private IResourceAuthorizationHandler<T> GetHandler<T>() where T : class
    {
        var handler = serviceProvider.GetService<IResourceAuthorizationHandler<T>>();

        if (handler is null)
            return serviceProvider.GetRequiredService<DefaultAuthorizationHandler<T>>();

        return handler;
    }
}
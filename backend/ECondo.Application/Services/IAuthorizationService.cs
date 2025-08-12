using ECondo.Domain.Authorization;

namespace ECondo.Application.Services;

public interface IAuthorizationService
{
    Task<bool> CanPerformActionAsync<T>(Guid userId, 
        Guid? resourceId,
        AccessLevel resourceAccess,
        CancellationToken cancellationToken = default)
        where T : class;
    

    Task<IQueryable<T>> ApplyDataFilterAsync<T>(IQueryable<T> query, 
        Guid userId,
        CancellationToken cancellationToken = default)
        where T : class;
}
namespace ECondo.Domain.Authorization;

public interface ICanDelete<T> : IResourcePolicy<T>
    where T : class
{
    AccessLevel IResourcePolicy.ResourceAction => AccessLevel.Delete;
}
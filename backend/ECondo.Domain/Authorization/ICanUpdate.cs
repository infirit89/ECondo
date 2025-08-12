namespace ECondo.Domain.Authorization;

public interface ICanUpdate<T> : IResourcePolicy<T>
    where T : class
{
    AccessLevel IResourcePolicy.ResourceAction => AccessLevel.Update;
}
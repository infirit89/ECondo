namespace ECondo.Domain.Authorization;

public interface ICanRead<T> : IResourcePolicy<T>
    where T : class
{
    AccessLevel IResourcePolicy.ResourceAction => AccessLevel.Read;
}
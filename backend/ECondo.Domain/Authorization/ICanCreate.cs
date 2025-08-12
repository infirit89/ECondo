namespace ECondo.Domain.Authorization;

public interface ICanCreate<T> : IResourcePolicy<T>
    where T : class
{
    AccessLevel IResourcePolicy.ResourceAction => AccessLevel.Create;
}
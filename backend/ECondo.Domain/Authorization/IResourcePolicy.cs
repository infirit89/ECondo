namespace ECondo.Domain.Authorization;

public interface IResourcePolicy<T> : IResourcePolicy where T : class
{
    Type IResourcePolicy.ResourceType => typeof(T);
}

public interface IResourcePolicy
{
    Type ResourceType { get; }
    Guid? ResourceId { get; }
    AccessLevel ResourceAction { get; }
}

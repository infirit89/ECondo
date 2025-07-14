namespace ECondo.Application.Shared;

public interface IAuthRequirement
{
    string Permission { get; }
}

public interface IAuthRequirementResource : IAuthRequirement
{
    ResourceContext? Resource { get; }
}
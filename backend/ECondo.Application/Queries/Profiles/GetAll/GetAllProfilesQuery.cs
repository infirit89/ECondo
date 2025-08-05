using ECondo.Domain.Authorization;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;

namespace ECondo.Application.Queries.Profiles.GetAll;

public record UserProfileResult(string FirstName, string MiddleName, string LastName, string Email);

public record GetAllProfilesQuery(int Page, int PageSize) :
    IQuery<PagedList<UserProfileResult>>, ICanRead<User>
{
    Guid? IResourcePolicy.ResourceId => null;
}
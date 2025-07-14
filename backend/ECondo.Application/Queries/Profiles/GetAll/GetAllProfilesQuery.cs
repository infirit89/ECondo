using ECondo.Application.Policies;
using ECondo.Application.Policies.Admin;
using ECondo.Domain.Shared;

namespace ECondo.Application.Queries.Profiles.GetAll;

public record UserProfileResult(string FirstName, string MiddleName, string LastName, string Email);

public record GetAllProfilesQuery(int Page, int PageSize) : 
    IQuery<PagedList<UserProfileResult>>, IIsAdmin;
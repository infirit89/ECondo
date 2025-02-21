using ECondo.Application.Repositories;
using ECondo.Domain.Profiles;
using ECondo.Domain.Users;

namespace ECondo.Application.Services;
public interface IUnitOfWork
{
    IRepository<ProfileDetails> ProfileDetailsRepository { get; }
    Task<bool> SaveChangesAsync();
}

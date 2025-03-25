using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Domain.Profiles;

namespace ECondo.Application.Services;
public interface IUnitOfWork
{
    IRepository<ProfileDetails> ProfileDetailsRepository { get; }
    IRepository<Building> BuildingRepository { get; }
    IRepository<Province> ProvinceRepository { get; }

    Task<bool> SaveChangesAsync();
}

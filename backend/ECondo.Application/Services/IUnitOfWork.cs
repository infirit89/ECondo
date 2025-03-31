using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Domain.Profiles;

namespace ECondo.Application.Services;
public interface IUnitOfWork
{
    IRepository<ProfileDetails> ProfileDetails { get; }
    IRepository<Building> Buildings { get; }
    IRepository<Province> Provinces { get; }
    IRepository<Entrance> Entrances { get; }

    Task<bool> SaveChangesAsync();
}


using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Profiles;
using ECondo.Domain.Provinces;
using ECondo.Infrastructure.Contexts;
using ECondo.Infrastructure.Repositories;

namespace ECondo.Infrastructure.Services;
internal class UnitOfWork(ECondoDbContext dbContext) : IUnitOfWork
{
    public IRepository<ProfileDetails> ProfileDetails { get; } =
        new GenericRepository<ProfileDetails>(dbContext);

    public IRepository<Building> Buildings { get; } =
        new GenericRepository<Building>(dbContext);

    public IRepository<Province> Provinces { get; } =
        new GenericRepository<Province>(dbContext);

    public IRepository<Entrance> Entrances { get; } =
        new GenericRepository<Entrance>(dbContext);

    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}


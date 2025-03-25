using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Profiles;
using ECondo.Infrastructure.Contexts;
using ECondo.Infrastructure.Repositories;

namespace ECondo.Infrastructure.Services;
internal class UnitOfWork(ECondoDbContext dbContext) : IUnitOfWork
{
    public IRepository<ProfileDetails> ProfileDetailsRepository { get; } =
        new GenericRepository<ProfileDetails>(dbContext);

    public IRepository<Building> BuildingRepository { get; } =
        new GenericRepository<Building>(dbContext);

    public IRepository<Province> ProvinceRepository { get; } =
        new GenericRepository<Province>(dbContext);

    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() > 0;
    }
}

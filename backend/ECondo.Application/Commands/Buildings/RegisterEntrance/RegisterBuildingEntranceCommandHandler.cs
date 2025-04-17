using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Provinces;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Buildings.RegisterEntrance;

internal sealed class RegisterBuildingEntranceCommandHandler(
    IApplicationDbContext dbContext,
    IUserContext userContext)
    : ICommandHandler<RegisterBuildingEntranceCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
        RegisterBuildingEntranceCommand request,
        CancellationToken cancellationToken)
    {
        var buildingResult = await GetOrCreateBuilding(request);

        if(!buildingResult.IsOk())
            return Result<EmptySuccess, Error>
                .Fail(buildingResult.ToError().Data);

        var building = buildingResult.ToSuccess().Data!;

        if(userContext.UserId is null)
            return Result<EmptySuccess, Error>.Fail(
                UserErrors.InvalidUser());

        var entrance = await dbContext.Entrances
            .AsNoTracking()
            .FirstOrDefaultAsync(e =>
                e.BuildingId == building.Id
                && e.Number == request.EntranceNumber,
                cancellationToken: cancellationToken); 

        if(entrance is not null)
            return Result<EmptySuccess, Error>
                .Fail(EntranceErrors.AlreadyExists(
                    building.Id, request.EntranceNumber));

        entrance = new Entrance
        {
            Building = building,
            Number = request.EntranceNumber,
            ManagerId = (Guid)userContext.UserId,
        };

        await dbContext
            .Entrances
            .AddAsync(entrance, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<EmptySuccess, Error>.Ok();
    }

    private bool IsBuildingEqual(
        Building building,
        RegisterBuildingEntranceCommand request)
        => building.Province.Name.ToLower().Trim() 
            == request.ProvinceName.ToLower().Trim() &&
           building.Municipality.ToLower().Trim() 
            == request.Municipality.ToLower().Trim() &&
           building.SettlementPlace.ToLower().Trim() 
            == request.SettlementPlace.ToLower().Trim() &&
           building.Neighborhood.ToLower().Trim() 
            == request.Neighborhood.ToLower().Trim() &&
           building.PostalCode.ToLower().Trim() 
            == request.PostalCode.ToLower().Trim() &&
           building.Street.ToLower().Trim() 
            == request.Street.ToLower().Trim() &&
           building.StreetNumber.ToLower().Trim() 
            == request.StreetNumber.ToLower().Trim() &&
           building.BuildingNumber.ToLower().Trim() 
            == request.BuildingNumber.ToLower().Trim();

    internal async Task<Result<Building, Error>> 
        GetOrCreateBuilding(RegisterBuildingEntranceCommand request)
    {
        var building = await dbContext.Buildings
            .AsNoTracking()
            .Include(b => b.Province)
            .FirstOrDefaultAsync(b => 
                IsBuildingEqual(b, request));

        if (building is not null) 
            return Result<Building, Error>.Ok(building);

        var province = await dbContext.Provinces
            .AsNoTracking()
            .FirstOrDefaultAsync(p => 
                p.Name == request.ProvinceName);

        if (province is null)
            return Result<Building, Error>
                .Fail(ProvinceErrors
                    .InvalidProvince(request.ProvinceName));

        building = new Building
        {
            Name = request.BuildingName,
            Province = province,
            Municipality = request.Municipality,
            SettlementPlace = request.SettlementPlace,
            Neighborhood = request.Neighborhood,
            PostalCode = request.PostalCode,
            Street = request.Street,
            StreetNumber = request.StreetNumber,
            BuildingNumber = request.BuildingNumber,
        };

        await dbContext.Buildings.AddAsync(building);

        return Result<Building, Error>.Ok(building);
    }
}

using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Provinces;
using ECondo.SharedKernel.Result;
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

        var entrance = await dbContext.Entrances
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
            ManagerId = userContext.UserId,
        };

        await dbContext
            .Entrances
            .AddAsync(entrance, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result<EmptySuccess, Error>.Ok();
    }

    internal async Task<Result<Building, Error>> 
        GetOrCreateBuilding(RegisterBuildingEntranceCommand request)
    {
        var building = await dbContext.Buildings
            .Include(b => b.Province)
            .FirstOrDefaultAsync(b =>
                b.Province.Name.ToLower().Trim()
                == request.ProvinceName.ToLower().Trim() &&
                b.Municipality.ToLower().Trim()
                == request.Municipality.ToLower().Trim() &&
                b.SettlementPlace.ToLower().Trim()
                == request.SettlementPlace.ToLower().Trim() &&
                b.Neighborhood.ToLower().Trim()
                == request.Neighborhood.ToLower().Trim() &&
                b.PostalCode.ToLower().Trim()
                == request.PostalCode.ToLower().Trim() &&
                b.Street.ToLower().Trim()
                == request.Street.ToLower().Trim() &&
                b.StreetNumber.ToLower().Trim()
                == request.StreetNumber.ToLower().Trim() &&
                b.BuildingNumber.ToLower().Trim()
                == request.BuildingNumber.ToLower().Trim());

        if (building is not null) 
            return Result<Building, Error>.Ok(building);

        var province = await dbContext.Provinces
            .FirstOrDefaultAsync(p => 
                p.Name == request.ProvinceName);

        if (province is null)
            return Result<Building, Error>
                .Fail(ProvinceErrors
                    .InvalidProvince(request.ProvinceName));

        building = new Building
        {
            Name = request.BuildingName,
            ProvinceId = province.Id,
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

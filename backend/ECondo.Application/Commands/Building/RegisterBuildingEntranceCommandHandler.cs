using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;

namespace ECondo.Application.Commands.Building;

internal sealed class RegisterBuildingEntranceCommandHandler(
    IUnitOfWork unitOfWork,
    IUserContext userContext)
    : IRequestHandler<RegisterBuildingEntranceCommand, Result<EmptySuccess, Error>>
{
    public async Task<Result<EmptySuccess, Error>> Handle(RegisterBuildingEntranceCommand request, CancellationToken cancellationToken)
    {
        var buildingResult = await GetOrCreateBuilding(request);

        if(!buildingResult.IsOk())
            return Result<EmptySuccess, Error>.Fail(buildingResult.ToError().Data);

        var building = buildingResult.ToSuccess().Data!;

        if(userContext.UserId is null)
            return Result<EmptySuccess, Error>.Fail(
                UserErrors.InvalidUser());

        var entrance = await unitOfWork.Entrances
            .FirstOrDefaultAsync(e =>
                e.BuildingId == building.Id
                && e.Number == request.EntranceNumber); 

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

        await unitOfWork.Entrances.InsertAsync(entrance);

        await unitOfWork.SaveChangesAsync();
        return Result<EmptySuccess, Error>.Ok();
    }

    internal async Task<Result<Domain.Buildings.Building, Error>> GetOrCreateBuilding(RegisterBuildingEntranceCommand request)
    {
        var building = await unitOfWork.Buildings
            .FirstOrDefaultAsync(b =>
                    b.Province.Name.ToLower().Trim() == request.ProvinceName.ToLower().Trim() &&
                    b.Municipality.ToLower().Trim() == request.Municipality.ToLower().Trim() &&
                    b.SettlementPlace.ToLower().Trim() == request.SettlementPlace.ToLower().Trim() &&
                    b.Neighborhood.ToLower().Trim() == request.Neighborhood.ToLower().Trim() &&
                    b.PostalCode.ToLower().Trim() == request.PostalCode.ToLower().Trim() &&
                    b.Street.ToLower().Trim() == request.Street.ToLower().Trim() &&
                    b.StreetNumber.ToLower().Trim() == request.StreetNumber.ToLower().Trim() &&
                    b.BuildingNumber.ToLower().Trim() == request.BuildingNumber.ToLower().Trim(),
                nameof(Domain.Buildings.Building.Province));

        if (building is not null) 
            return Result<Domain.Buildings.Building, Error>.Ok(building);

        var province = await unitOfWork.Provinces
            .FirstOrDefaultAsync(p => p.Name == request.ProvinceName);

        if (province is null)
            return Result<Domain.Buildings.Building, Error>
                .Fail(ProvinceErrors.InvalidProvince(request.ProvinceName));

        building = new Domain.Buildings.Building()
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

        await unitOfWork.Buildings.InsertAsync(building);

        return Result<Domain.Buildings.Building, Error>.Ok(building);
    }
}

using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Domain.Provinces;
using ECondo.Domain.Shared;
using ECondo.SharedKernel.Result;
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.Commands.Buildings.Update;

internal sealed class UpdateBuildingCommandHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<UpdateBuildingCommand>
{
    public async Task<Result<EmptySuccess, Error>> Handle(
            UpdateBuildingCommand request,
            CancellationToken cancellationToken)
    {
        Building? building = await dbContext
            .Buildings
            .FirstOrDefaultAsync(b => 
                b.Id == request.BuildingId,
                cancellationToken: cancellationToken);

        if(building is null)
            return Result<EmptySuccess, Error>
                .Fail(BuildingErrors
                    .InvalidBuilding(request.BuildingId));

        if (building.Province.Name != request.ProvinceName)
        {
            Province? province = await dbContext
                .Provinces
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    p => p.Name == request.ProvinceName,
                    cancellationToken: cancellationToken);
            
            if(province is null)
                return Result<EmptySuccess, Error>
                    .Fail(ProvinceErrors
                        .InvalidProvince(request.ProvinceName));
        }

        building.Name = request.BuildingName;
        building.Municipality = request.Municipality;
        building.SettlementPlace = request.SettlementPlace;
        building.Neighborhood = request.Neighborhood;
        building.PostalCode = request.PostalCode;
        building.Street = request.Street;
        building.StreetNumber = request.StreetNumber;
        building.BuildingNumber = request.BuildingNumber;

        dbContext.Buildings.Update(building);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<EmptySuccess, Error>.Ok();
    }
}
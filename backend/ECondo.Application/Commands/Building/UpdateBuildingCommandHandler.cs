using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Provinces;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using MediatR;

namespace ECondo.Application.Commands.Building;

internal sealed class UpdateBuildingCommandHandler(
    IUserContext userContext,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBuildingCommand, Result<EmptySuccess, Error>>
{
    public async Task<Result<EmptySuccess, Error>>
        Handle(
            UpdateBuildingCommand request,
            CancellationToken cancellationToken)
    {
        if(userContext.UserId is null)
            return Result<EmptySuccess, Error>
                .Fail(UserErrors.InvalidUser());

        Domain.Buildings.Building? building = await unitOfWork
            .Buildings
            .FirstOrDefaultAsync(b => b.Id == request.BuildingId);

        if(building is null)
            return Result<EmptySuccess, Error>
                .Fail(BuildingErrors.InvalidBuilding(request.BuildingId));

        if (building.Province.Name != request.ProvinceName)
        {
            Province? province = await unitOfWork
                .Provinces
                .FirstOrDefaultAsync(
                    p => p.Name == request.ProvinceName);
            
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

        unitOfWork.Buildings.Update(building);
        await unitOfWork.SaveChangesAsync();

        return Result<EmptySuccess, Error>.Ok();
    }
}
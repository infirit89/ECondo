using ECondo.Application.Commands.Buildings.Update;
using ECondo.Application.Repositories;
using ECondo.Domain.Buildings;
using ECondo.Domain.Provinces;
using ECondo.Domain.Shared;
using ECondo.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ECondo.Application.IntegrationTests.Commands.Buildings.Update;

public class UpdateBuildingCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly UpdateBuildingCommandHandler _handler;

    public UpdateBuildingCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _handler = new UpdateBuildingCommandHandler(_dbContext);
    }

    [Fact]
    public async Task Handle_ShouldUpdateBuilding_WhenBuildingExistsAndProvinceIsValid()
    {
        // Arrange
        var province = new Province { Id = Guid.NewGuid(), Name = "ValidProvince" };
        var building = new Building
        {
            Id = Guid.NewGuid(),
            Name = "OldBuildingName",
            Province = province,
            Municipality = "OldMunicipality",
            SettlementPlace = "OldSettlement",
            Neighborhood = "OldNeighborhood",
            PostalCode = "12345",
            Street = "OldStreet",
            StreetNumber = "1",
            BuildingNumber = "A"
        };

        _dbContext.Provinces.Add(province);
        _dbContext.Buildings.Add(building);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateBuildingCommand(
            building.Id,
            "NewBuildingName",
            "ValidProvince",
            "NewMunicipality",
            "NewSettlement",
            "NewNeighborhood",
            "54321",
            "NewStreet",
            "10",
            "B"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        var updatedBuilding = await _dbContext.Buildings.FirstOrDefaultAsync(b => b.Id == building.Id);
        updatedBuilding.Should().NotBeNull();
        updatedBuilding!.Name.Should().Be("NewBuildingName");
        updatedBuilding.Municipality.Should().Be("NewMunicipality");
        updatedBuilding.SettlementPlace.Should().Be("NewSettlement");
        updatedBuilding.Neighborhood.Should().Be("NewNeighborhood");
        updatedBuilding.PostalCode.Should().Be("54321");
        updatedBuilding.Street.Should().Be("NewStreet");
        updatedBuilding.StreetNumber.Should().Be("10");
        updatedBuilding.BuildingNumber.Should().Be("B");
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenBuildingDoesNotExist()
    {
        // Arrange
        var command = new UpdateBuildingCommand(
            Guid.NewGuid(),
            "NewBuildingName",
            "ValidProvince",
            "NewMunicipality",
            "NewSettlement",
            "NewNeighborhood",
            "54321",
            "NewStreet",
            "10",
            "B"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenProvinceDoesNotExist()
    {
        // Arrange
        var building = new Building
        {
            Id = Guid.NewGuid(),
            Name = "OldBuildingName",
            Province = new Province { Id = Guid.NewGuid(), Name = "OldProvince" },
            Municipality = "OldMunicipality",
            SettlementPlace = "OldSettlement",
            Neighborhood = "OldNeighborhood",
            PostalCode = "12345",
            Street = "OldStreet",
            StreetNumber = "1",
            BuildingNumber = "A"
        };

        _dbContext.Buildings.Add(building);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateBuildingCommand(
            building.Id,
            "NewBuildingName",
            "NonExistentProvince",
            "NewMunicipality",
            "NewSettlement",
            "NewNeighborhood",
            "54321",
            "NewStreet",
            "10",
            "B"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Handle_ShouldWorkCorrectly_WhenNoChangesAreMade()
    {
        // Arrange
        var province = new Province { Id = Guid.NewGuid(), Name = "ValidProvince" };
        var building = new Building
        {
            Id = Guid.NewGuid(),
            Name = "BuildingName",
            Province = province,
            Municipality = "Municipality",
            SettlementPlace = "SettlementPlace",
            Neighborhood = "Neighborhood",
            PostalCode = "12345",
            Street = "Street",
            StreetNumber = "1",
            BuildingNumber = "A"
        };

        _dbContext.Provinces.Add(province);
        _dbContext.Buildings.Add(building);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateBuildingCommand(
            building.Id,
            "BuildingName",
            "ValidProvince",
            "Municipality",
            "SettlementPlace",
            "Neighborhood",
            "12345",
            "Street",
            "1",
            "A"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        var unchangedBuilding = await _dbContext.Buildings.FirstOrDefaultAsync(b => b.Id == building.Id);
        unchangedBuilding.Should().NotBeNull();
        unchangedBuilding!.Name.Should().Be("BuildingName");
        unchangedBuilding.Municipality.Should().Be("Municipality");
        unchangedBuilding.SettlementPlace.Should().Be("SettlementPlace");
        unchangedBuilding.Neighborhood.Should().Be("Neighborhood");
        unchangedBuilding.PostalCode.Should().Be("12345");
        unchangedBuilding.Street.Should().Be("Street");
        unchangedBuilding.StreetNumber.Should().Be("1");
        unchangedBuilding.BuildingNumber.Should().Be("A");
    }
}

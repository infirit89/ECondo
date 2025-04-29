using ECondo.Application.Commands.Buildings.RegisterEntrance;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Provinces;
using ECondo.Domain.Shared;
using ECondo.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace ECondo.Application.IntegrationTests.Commands.Buildings.RegisterEntrance;

public class RegisterBuildingEntranceCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly RegisterBuildingEntranceCommandHandler _handler;

    public RegisterBuildingEntranceCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _userContext = Substitute.For<IUserContext>();
        _userContext.UserId.Returns(Guid.NewGuid());
        _handler = new RegisterBuildingEntranceCommandHandler(_dbContext, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldRegisterEntrance_WhenBuildingExists()
    {
        // Arrange
        var province = new Province { Id = Guid.NewGuid(), Name = "TestProvince" };
        var building = new Building
        {
            Id = Guid.NewGuid(),
            Name = "TestBuilding",
            Province = province,
            Municipality = "TestMunicipality",
            SettlementPlace = "TestSettlement",
            Neighborhood = "TestNeighborhood",
            PostalCode = "12345",
            Street = "TestStreet",
            StreetNumber = "1",
            BuildingNumber = "A"
        };

        _dbContext.Provinces.Add(province);
        _dbContext.Buildings.Add(building);
        await _dbContext.SaveChangesAsync();

        var command = new RegisterBuildingEntranceCommand(
            "TestBuilding",
            "TestProvince",
            "TestMunicipality",
            "TestSettlement",
            "TestNeighborhood",
            "12345",
            "TestStreet",
            "1",
            "A",
            "Entrance1");
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        var entrance = await _dbContext.Entrances.FirstOrDefaultAsync(e => e.Number == "Entrance1");
        entrance.Should().NotBeNull();
        entrance!.BuildingId.Should().Be(building.Id);
    }

    [Fact]
    public async Task Handle_ShouldCreateBuildingAndRegisterEntrance_WhenBuildingDoesNotExist()
    {
        // Arrange
        var province = new Province { Id = Guid.NewGuid(), Name = "TestProvince" };
        _dbContext.Provinces.Add(province);
        await _dbContext.SaveChangesAsync();

        var command = new RegisterBuildingEntranceCommand(
            "TestBuilding",
            "TestProvince",
            "NewMunicipality",
            "NewSettlement",
            "NewNeighborhood",
            "54321",
            "NewStreet",
            "10",
            "B",
            "Entrance2"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        var building = await _dbContext.Buildings.FirstOrDefaultAsync(b => b.Municipality == "NewMunicipality");
        building.Should().NotBeNull();
        var entrance = await _dbContext.Entrances.FirstOrDefaultAsync(e => e.Number == "Entrance2");
        entrance.Should().NotBeNull();
        entrance!.BuildingId.Should().Be(building!.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenEntranceAlreadyExists()
    {
        // Arrange
        var province = new Province { Id = Guid.NewGuid(), Name = "TestProvince" };
        var building = new Building
        {
            Id = Guid.NewGuid(),
            Name = "TestBuilding",
            Province = province,
            Municipality = "TestMunicipality",
            SettlementPlace = "TestSettlement",
            Neighborhood = "TestNeighborhood",
            PostalCode = "12345",
            Street = "TestStreet",
            StreetNumber = "1",
            BuildingNumber = "A"
        };

        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            Building = building,
            Number = "Entrance1"
        };

        _dbContext.Provinces.Add(province);
        _dbContext.Buildings.Add(building);
        _dbContext.Entrances.Add(entrance);
        await _dbContext.SaveChangesAsync();

        var command = new RegisterBuildingEntranceCommand(
            "TestBuilding",
            "TestProvince",
            "TestMunicipality",
            "TestSettlement",
            "TestNeighborhood",
            "12345",
            "TestStreet",
            "1",
            "A",
            "Entrance1"
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
        var command = new RegisterBuildingEntranceCommand(
            "TestBuilding",
            "NonExistentProvince",
            "TestMunicipality",
            "TestSettlement",
            "TestNeighborhood",
            "12345",
            "TestStreet",
            "1",
            "A",
            "Entrance1"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }
}

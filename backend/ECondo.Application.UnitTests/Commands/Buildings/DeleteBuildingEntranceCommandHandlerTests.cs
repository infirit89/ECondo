using ECondo.Application.Commands.Buildings.Delete;
using ECondo.Application.Repositories;
using ECondo.Application.UnitTests.Helper;
using ECondo.Domain.Buildings;
using FluentAssertions;
using NSubstitute;

namespace ECondo.Application.UnitTests.Commands.Buildings.Delete;

public class DeleteBuildingEntranceCommandHandlerTests
{
    private readonly IApplicationDbContext _mockDbContext;
    private readonly DeleteBuildingEntranceCommandHandler _handler;

    public DeleteBuildingEntranceCommandHandlerTests()
    {
        _mockDbContext = Substitute.For<IApplicationDbContext>();
        _handler = new DeleteBuildingEntranceCommandHandler(_mockDbContext);
    }

    [Fact]
    public async Task Handle_ShouldDeleteEntranceAndRelatedEntities_WhenEntranceExists()
    {
        // Arrange
        var entranceId = Guid.NewGuid();
        var command = new DeleteBuildingEntranceCommand(entranceId);
        var propertyOccupants = new List<PropertyOccupant>()
        {
            new()
            {
                Id = Guid.NewGuid(),
            }
        }.AsQueryable();

        var properties = new List<Property>()
        {
            new()
            {
                PropertyOccupants = propertyOccupants.ToHashSet(),
            }
        }.AsQueryable();
        
        var entrances = new List<Entrance>()
        {
            new()
            {
                Id = entranceId,
                Properties = properties.ToHashSet(),
            }
        }.AsQueryable();

        var mockEntranceSet = DbSetMockHelper.CreateMockDbSet(entrances);
        var mockPropertySet = DbSetMockHelper.CreateMockDbSet(properties);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        _mockDbContext.Entrances.Returns(mockEntranceSet);
        _mockDbContext.Properties.Returns(mockPropertySet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        _mockDbContext.PropertyOccupants.Received(1).RemoveRange(Arg.Any<IEnumerable<PropertyOccupant>>());
        _mockDbContext.Properties.Received(1).RemoveRange(Arg.Any<IEnumerable<Property>>());
        _mockDbContext.Entrances.Received(1).Remove(entrances.First());
        await _mockDbContext.Received(1).SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEntranceNotFound()
    {
        // Arrange
        var entranceId = Guid.NewGuid();
        var command = new DeleteBuildingEntranceCommand(entranceId);
        var entrances = new List<Entrance>().AsQueryable();
        var mockEntranceSet = DbSetMockHelper.CreateMockDbSet(entrances);
        _mockDbContext.Entrances.Returns(mockEntranceSet);

        // Act
        var act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}


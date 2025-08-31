using ECondo.Application.Authorization;
using ECondo.Application.Repositories;
using ECondo.Application.UnitTests.Helper;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.Domain.Users;
using FluentAssertions;
using NSubstitute;

namespace ECondo.Application.UnitTests.Authorization;

public class PropertyAuthorizationHandlerTests
{
    private readonly IApplicationDbContext _mockDbContext;
    private readonly PropertyAuthorizationHandler _handler;

    public PropertyAuthorizationHandlerTests()
    {
        _mockDbContext = Substitute.For<IApplicationDbContext>();
        _handler = new PropertyAuthorizationHandler(_mockDbContext);
    }

    [Fact]
    public async Task GetAccessLevelAsync_AdminUser_ReturnsAllAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();

        var userRoles = new List<UserRole>
        {
            new() { UserId = userId, Role = new Role { Name = Role.Admin } }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);

        _mockDbContext.UserRoles.Returns(mockUserRoleSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, propertyId);

        // Assert
        result.Should().Be(AccessLevel.All);
    }

    [Fact]
    public async Task GetAccessLevelAsync_NoResourceId_ReturnsReadAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userRoles = new List<UserRole>().AsQueryable();
        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, null);

        // Assert
        result.Should().Be(AccessLevel.Read);
    }

    [Fact]
    public async Task GetAccessLevelAsync_EntranceManager_ReturnsAllAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();

        var userRoles = new List<UserRole>().AsQueryable();
        var properties = new List<Property>
        {
            new() 
            { 
                Id = propertyId, 
                Entrance = new Entrance { ManagerId = userId }
            }
        }.AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>().AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockPropertySet = DbSetMockHelper.CreateMockDbSet(properties);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Properties.Returns(mockPropertySet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, propertyId);

        // Assert
        result.Should().Be(AccessLevel.All);
    }

    [Fact]
    public async Task GetAccessLevelAsync_PropertyOccupant_ReturnsReadAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var managerId = Guid.NewGuid();

        var userRoles = new List<UserRole>().AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>
        {
            new() { UserId = userId, PropertyId = propertyId }
        }.AsQueryable();
        var properties = new List<Property>
        {
            new() 
            { 
                Id = propertyId, 
                Entrance = new Entrance { ManagerId = managerId },
                PropertyOccupants = propertyOccupants.ToHashSet(),
            }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockPropertySet = DbSetMockHelper.CreateMockDbSet(properties);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Properties.Returns(mockPropertySet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, propertyId);

        // Assert
        result.Should().Be(AccessLevel.Read);
    }

    [Fact]
    public async Task GetAccessLevelAsync_NotManagerNotOccupant_ReturnsNoAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var managerId = Guid.NewGuid();

        var userRoles = new List<UserRole>().AsQueryable();
        var properties = new List<Property>
        {
            new() 
            { 
                Id = propertyId, 
                Entrance = new Entrance { ManagerId = managerId }
            }
        }.AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>().AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockPropertySet = DbSetMockHelper.CreateMockDbSet(properties);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Properties.Returns(mockPropertySet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, propertyId);

        // Assert
        result.Should().Be(AccessLevel.None);
    }
}
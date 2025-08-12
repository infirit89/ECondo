using ECondo.Application.Authorization;
using ECondo.Application.Repositories;
using ECondo.Application.UnitTests.Helper;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.Domain.Users;
using FluentAssertions;
using NSubstitute;

namespace ECondo.Application.UnitTests.Authorization;

public class EntranceAuthorizationHandlerTests
{
    private readonly IApplicationDbContext _mockDbContext;
    private readonly EntranceAuthorizationHandler _handler;

    public EntranceAuthorizationHandlerTests()
    {
        _mockDbContext = Substitute.For<IApplicationDbContext>();
        _handler = new EntranceAuthorizationHandler(_mockDbContext);
    }

    [Fact]
    public async Task GetAccessLevelAsync_AdminUser_ReturnsAllAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var entranceId = Guid.NewGuid();
        
        var userRoles = new List<UserRole>
        {
            new() { UserId = userId, Role = new Role { Name = Role.Admin } }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, entranceId);

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
        var entranceId = Guid.NewGuid();
        
        var userRoles = new List<UserRole>().AsQueryable();
        var entrances = new List<Entrance>
        {
            new() 
            { 
                Id = entranceId, 
                ManagerId = userId
            }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockEntranceSet = DbSetMockHelper.CreateMockDbSet(entrances);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Entrances.Returns(mockEntranceSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, entranceId);

        // Assert
        result.Should().Be(AccessLevel.All);
    }

    [Fact]
    public async Task GetAccessLevelAsync_NotManager_ReturnsNoAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var entranceId = Guid.NewGuid();
        var managerId = Guid.NewGuid();
        
        var userRoles = new List<UserRole>().AsQueryable();
        var entrances = new List<Entrance>
        {
            new() 
            { 
                Id = entranceId, 
                ManagerId = managerId
            }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockEntranceSet = DbSetMockHelper.CreateMockDbSet(entrances);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Entrances.Returns(mockEntranceSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, entranceId);

        // Assert
        result.Should().Be(AccessLevel.None);
    }

    [Fact]
    public async Task ApplyDataFilterAsync_AdminUser_ReturnsUnfilteredQuery()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var entrances = new List<Entrance>
        {
            new() { Id = Guid.NewGuid(), ManagerId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), ManagerId = Guid.NewGuid() }
        }.AsQueryable();
        
        var userRoles = new List<UserRole>
        {
            new() { UserId = userId, Role = new Role { Name = Role.Admin } }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);

        // Act
        var result = await _handler.ApplyDataFilterAsync(entrances, userId);

        // Assert
        result.Count().Should().Be(2);
    }

    [Fact]
    public async Task ApplyDataFilterAsync_NonAdminUser_ReturnsFilteredQuery()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var entrances = new List<Entrance>
        {
            new() { Id = Guid.NewGuid(), ManagerId = userId },
            new() { Id = Guid.NewGuid(), ManagerId = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), ManagerId = Guid.NewGuid() }
        }.AsQueryable();
        
        var userRoles = new List<UserRole>().AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);

        // Act
        var result = await _handler.ApplyDataFilterAsync(entrances, userId);

        // Assert
        result.Count().Should().Be(1);
    }
}
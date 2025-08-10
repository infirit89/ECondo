using ECondo.Application.Authorization;
using ECondo.Application.Repositories;
using ECondo.Application.UnitTests.Helper;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.Domain.Users;
using FluentAssertions;
using NSubstitute;

namespace ECondo.Application.UnitTests.Authorization;

public class OccupantAuthorizationHandlerUnitTests
{
    private readonly IApplicationDbContext _mockDbContext;
    private readonly OccupantAuthorizationHandler _handler;

    public OccupantAuthorizationHandlerUnitTests()
    {
        _mockDbContext = Substitute.For<IApplicationDbContext>();
        _handler = new OccupantAuthorizationHandler(_mockDbContext);
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
        Assert.Equal(AccessLevel.All, result);
    }
    
    [Fact]
    public async Task GetAccessLevelAsync_NoResourceId_ReturnsNoneAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var userRoles = new List<UserRole>().AsQueryable();
        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, null);

        // Assert
        Assert.Equal(AccessLevel.None, result);
    }
    
    [Fact]
    public async Task GetAccessLevelAsync_EntranceManager_ReturnsAllAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var occupantId = Guid.NewGuid();

        var userRoles = new List<UserRole>().AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>()
        {
            new()
            {
                Id = occupantId,
                Property = new Property
                {
                    Entrance = new Entrance
                    {
                        ManagerId = userId
                    }
                },
                OccupantType = new OccupantType
                {
                    Name = OccupantType.TenantType
                }
            }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, occupantId);

        // Assert
        Assert.Equal(AccessLevel.All, result);
    }
    
    [Fact]
    public async Task GetAccessLevelAsync_PropertyOccupant_ReturnsReadAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var managerId = Guid.NewGuid();
        var propertyOccupantId = Guid.NewGuid();

        var userRoles = new List<UserRole>().AsQueryable();
        var properties = new List<Property>
        {
            new() 
            { 
                Id = propertyId, 
                Entrance = new Entrance { ManagerId = managerId }
            }
        }.AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>
        {
            new()
            {
                Id = propertyOccupantId,
                UserId = userId, 
                Property = properties.First(),
                OccupantType = new OccupantType
                {
                    Name = OccupantType.TenantType
                }
                
            }
        }.AsQueryable();

        properties.First().PropertyOccupants = propertyOccupants.ToHashSet();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockPropertySet = DbSetMockHelper.CreateMockDbSet(properties);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Properties.Returns(mockPropertySet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, propertyOccupantId);

        // Assert
        Assert.Equal(AccessLevel.Read, result);
    }
    
    [Fact]
    public async Task GetAccessLevelAsync_PropertyOwner_ReturnsAllAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var managerId = Guid.NewGuid();
        var occupantId = Guid.NewGuid();

        var userRoles = new List<UserRole>().AsQueryable();
        
        var properties = new List<Property>
        {
            new() 
            { 
                Id = propertyId, 
                Entrance = new Entrance { ManagerId = managerId }
            }
        }.AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>
        {
            new()
            {
                Id = occupantId,
                Property = properties.First(),
                OccupantType = new OccupantType
                {
                    Name = OccupantType.TenantType
                }
            },
            new()
            {
                UserId = userId,
                Property = properties.First(),
                OccupantType = new OccupantType
                {
                    Name = OccupantType.OwnerType
                }
            }
        }.AsQueryable();

        properties.First().PropertyOccupants = propertyOccupants.ToHashSet();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockPropertySet = DbSetMockHelper.CreateMockDbSet(properties);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Properties.Returns(mockPropertySet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, occupantId);

        // Assert
        result.Should().Be(AccessLevel.All);
    }
    
    [Fact]
    public async Task GetAccessLevelAsync_NotManagerNotOccupant_ReturnsNoAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var propertyId = Guid.NewGuid();
        var propertyId2 = Guid.NewGuid();
        var managerId = Guid.NewGuid();
        var occupantId = Guid.NewGuid();

        var userRoles = new List<UserRole>().AsQueryable();
        var entrance = new Entrance()
        {
            ManagerId = managerId,
        };
        var properties = new List<Property>
        {
            new() 
            { 
                Id = propertyId, 
                Entrance = entrance
            },
            new()
            {
                Id = propertyId2,
                Entrance = entrance
            }
        }.AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>
        {
            new()
            {
                Id = occupantId,
                Property = properties.First(),
                OccupantType = new OccupantType
                {
                    Name = OccupantType.TenantType
                }
            },
            new()
            {
                UserId = userId,
                Property = properties.ElementAt(1),
                OccupantType = new OccupantType
                {
                    Name = OccupantType.OwnerType
                }
            }
        }.AsQueryable();

        properties.First().PropertyOccupants = new HashSet<PropertyOccupant>([propertyOccupants.First()]);
        properties.ElementAt(1).PropertyOccupants = new HashSet<PropertyOccupant>([propertyOccupants.ElementAt(1)]);

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockPropertySet = DbSetMockHelper.CreateMockDbSet(properties);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Properties.Returns(mockPropertySet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, occupantId);

        // Assert
        result.Should().Be(AccessLevel.None);
    }
}

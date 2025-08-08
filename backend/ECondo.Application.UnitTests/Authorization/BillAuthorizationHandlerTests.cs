using ECondo.Application.Authorization;
using ECondo.Application.Repositories;
using ECondo.Application.UnitTests.Helper;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.Domain.Payments;
using ECondo.Domain.Users;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ECondo.Application.UnitTests.Authorization;

public class BillAuthorizationHandlerTests
{
    private readonly IApplicationDbContext _mockDbContext;
    private readonly BillAuthorizationHandler _handler;

    public BillAuthorizationHandlerTests()
    {
        _mockDbContext = Substitute.For<IApplicationDbContext>();
        _handler = new BillAuthorizationHandler(_mockDbContext);
    }

    [Fact]
    public async Task GetAccessLevelAsync_AdminUser_ReturnsAllAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var billId = Guid.NewGuid();
        
        var userRoles = new List<UserRole>
        {
            new() { UserId = userId, Role = new Role { Name = Role.Admin } }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, billId);

        // Assert
        Assert.Equal(AccessLevel.All, result);
    }

    [Fact]
    public async Task GetAccessLevelAsync_NoResourceId_ReturnsNoAccess()
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
        var billId = Guid.NewGuid();
        
        var userRoles = new List<UserRole>().AsQueryable();
        var bills = new List<Bill>
        {
            new() 
            { 
                Id = billId, 
                Entrance = new Entrance { ManagerId = userId }
            }
        }.AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>().AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockBillSet = DbSetMockHelper.CreateMockDbSet(bills);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Bills.Returns(mockBillSet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, billId);

        // Assert
        Assert.Equal(AccessLevel.All, result);
    }

    [Fact]
    public async Task GetAccessLevelAsync_PropertyOccupantInEntrance_ReturnsReadAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var billId = Guid.NewGuid();
        var managerId = Guid.NewGuid();
        
        
        var userRoles = new List<UserRole>().AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>
        {
            new() 
            { 
                UserId = userId,
            }
        }.AsQueryable();
        
        var bills = new List<Bill>
        {
            new() 
            { 
                Id = billId, 
                Entrance = new Entrance
                {
                    ManagerId = managerId,
                    Properties =
                    [
                        new()
                        {
                            PropertyOccupants = propertyOccupants.ToHashSet()
                        }
                    ]
                }
            }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockBillSet = DbSetMockHelper.CreateMockDbSet(bills);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Bills.Returns(mockBillSet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, billId);

        // Assert
        Assert.Equal(AccessLevel.Read, result);
    }

    [Fact]
    public async Task GetAccessLevelAsync_NotManagerNotOccupant_ReturnsNoAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var billId = Guid.NewGuid();
        var managerId = Guid.NewGuid();
        
        var userRoles = new List<UserRole>().AsQueryable();
        var bills = new List<Bill>
        {
            new() 
            { 
                Id = billId, 
                Entrance = new Entrance { ManagerId = managerId }
            }
        }.AsQueryable();
        var propertyOccupants = new List<PropertyOccupant>().AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        var mockBillSet = DbSetMockHelper.CreateMockDbSet(bills);
        var mockPropertyOccupantSet = DbSetMockHelper.CreateMockDbSet(propertyOccupants);
        
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);
        _mockDbContext.Bills.Returns(mockBillSet);
        _mockDbContext.PropertyOccupants.Returns(mockPropertyOccupantSet);

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, billId);

        // Assert
        Assert.Equal(AccessLevel.None, result);
    }

    [Fact]
    public async Task ApplyDataFilterAsync_AdminUser_ReturnsUnfilteredQuery()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bills = new List<Bill>
        {
            new() { Id = Guid.NewGuid(), Entrance = new Entrance { ManagerId = Guid.NewGuid(), Properties = new HashSet<Property>() } },
            new() { Id = Guid.NewGuid(), Entrance = new Entrance { ManagerId = Guid.NewGuid(), Properties = new HashSet<Property>() } }
        }.AsQueryable();
        
        var userRoles = new List<UserRole>
        {
            new() { UserId = userId, Role = new Role { Name = Role.Admin } }
        }.AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);

        // Act
        var result = await _handler.ApplyDataFilterAsync(bills, userId);

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task ApplyDataFilterAsync_NonAdminUser_ReturnsFilteredQuery()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bills = new List<Bill>
        {
            new() 
            { 
                Id = Guid.NewGuid(), 
                Entrance = new Entrance 
                { 
                    ManagerId = userId,
                    Properties = new HashSet<Property>()
                }
            },
            new() 
            { 
                Id = Guid.NewGuid(), 
                Entrance = new Entrance 
                { 
                    ManagerId = Guid.NewGuid(),
                    Properties = new HashSet<Property>
                    {
                        new() { PropertyOccupants = new HashSet<PropertyOccupant> { new() { UserId = userId } } }
                    }
                }
            },
            new() 
            { 
                Id = Guid.NewGuid(), 
                Entrance = new Entrance 
                { 
                    ManagerId = Guid.NewGuid(),
                    Properties = new HashSet<Property>()
                }
            }
        }.AsQueryable();
        
        var userRoles = new List<UserRole>().AsQueryable();

        var mockUserRoleSet = DbSetMockHelper.CreateMockDbSet(userRoles);
        _mockDbContext.UserRoles.Returns(mockUserRoleSet);

        // Act
        var result = await _handler.ApplyDataFilterAsync(bills, userId);

        // Assert
        Assert.Equal(2, result.Count());
    }
}
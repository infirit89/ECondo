using ECondo.Application.Authorization;
using ECondo.Application.Repositories;
using ECondo.Application.UnitTests.Helper;
using ECondo.Domain.Authorization;
using ECondo.Domain.Buildings;
using ECondo.Domain.Payments;
using ECondo.Domain.Users;
using FluentAssertions;
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
        result
            .Should()
            .Be(AccessLevel.All);
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
        result
            .Should()
            .Be(AccessLevel.None);
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
                Entrance = new Entrance
                {
                    ManagerId = userId,
                    Number = "1",
                }
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
        result
            .Should()
            .Be(AccessLevel.All);
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
        result
            .Should()
            .Be(AccessLevel.Read);
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
        result
            .Should()
            .Be(AccessLevel.None);
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
        result
            .Count()
            .Should()
            .Be(2);
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
                    Properties = [
                        new Property
                        {
                            PropertyOccupants = [
                                new PropertyOccupant
                                {
                                    UserId = userId
                                }
                            ]
                        }
                    ]
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
        result
            .Count()
            .Should()
            .Be(2);
    }
}
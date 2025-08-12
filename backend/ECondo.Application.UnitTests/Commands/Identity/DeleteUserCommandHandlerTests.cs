using ECondo.Application.Commands.Identity.Delete;
using ECondo.Application.Repositories;
using ECondo.Application.UnitTests.Helper;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using NSubstitute;

namespace ECondo.Application.UnitTests.Commands.Identity.Delete;

public class DeleteUserCommandHandlerTests
{
    private readonly IApplicationDbContext _mockDbContext;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _mockDbContext = Substitute.For<IApplicationDbContext>();
        _handler = new DeleteUserCommandHandler(_mockDbContext);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidUserError_WhenUserNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteUserCommand(userId);
        var users = new List<User>().AsQueryable();

        var mockUserSet = DbSetMockHelper.CreateMockDbSet(users);
        _mockDbContext.Users.Returns(mockUserSet);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
        result.ToError().Data!.Code.Should().Be("Users.NotFound");
    }

    [Fact]
    public async Task Handle_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new DeleteUserCommand(userId);
        var users = new List<User>()
        {
            new()
            {
                Id = userId,
            }
        }.AsQueryable();
        var mockUserSet = DbSetMockHelper.CreateMockDbSet(users);
        _mockDbContext.Users.Returns(mockUserSet);
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        _mockDbContext.Users.Received(1).Remove(users.First());
        await _mockDbContext.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}


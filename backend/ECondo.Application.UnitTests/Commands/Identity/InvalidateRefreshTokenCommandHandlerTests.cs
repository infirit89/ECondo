using ECondo.Application.Commands.Identity.InvalidateRefreshToken;
using ECondo.Application.Services;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using NSubstitute;

namespace ECondo.Application.UnitTests.Commands.Identity.InvalidateRefreshToken;

public class InvalidateRefreshTokenCommandHandlerTests
{
    private readonly IAuthTokenService _authService;
    private readonly InvalidateRefreshTokenCommandHandler _handler;

    public InvalidateRefreshTokenCommandHandlerTests()
    {
        _authService = Substitute.For<IAuthTokenService>();
        _handler = new InvalidateRefreshTokenCommandHandler(_authService);
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidRefreshTokenError_WhenTokenIsNullOrEmpty()
    {
        // Arrange
        var command = new InvalidateRefreshTokenCommand(string.Empty);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
        result.ToError().Data!.Code.Should().Be("Users.InvalidRefreshToken");
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidRefreshTokenError_WhenTokenDoesNotExist()
    {
        // Arrange
        var command = new InvalidateRefreshTokenCommand("nonexistent-token");
        _authService.RefreshTokenExistsAsync(command.RefreshToken).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
        result.ToError().Data!.Code.Should().Be("Users.InvalidRefreshToken");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenTokenIsInvalidatedSuccessfully()
    {
        // Arrange
        var command = new InvalidateRefreshTokenCommand("valid-token");
        _authService.RefreshTokenExistsAsync(command.RefreshToken).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        await _authService.Received(1).RemoveRefreshTokenAsync(command.RefreshToken);
    }
}

using ECondo.Application.Commands.Identity.InvalidateRefreshToken;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECondo.Application.UnitTests.Commands.Identity
{
    public class InvalidateRefreshTokenCommandHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthTokenService _authTokenService;
        private readonly InvalidateRefreshTokenCommandHandler _handler;

        public InvalidateRefreshTokenCommandHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _authTokenService = Substitute.For<IAuthTokenService>();
            _handler = new InvalidateRefreshTokenCommandHandler(_userManager, _authTokenService);
        }

        [Fact]
        public async Task Handle_RefreshTokenIsNullOrEmpty_ReturnsInvalidRefreshTokenError()
        {
            // Arrange
            var command = new InvalidateRefreshTokenCommand("testUser", "");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            result.As<Result<EmptySuccess, Error>.Error>().Data.Code.Should().Be("Users.InvalidRefreshToken");
        }

        [Fact]
        public async Task Handle_RefreshTokenDoesNotExist_ReturnsInvalidRefreshTokenError()
        {
            // Arrange
            var command = new InvalidateRefreshTokenCommand("testUser", "invalidToken");
            _authTokenService.RefreshTokenExistsAsync(command.RefreshToken).Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            result.As<Result<EmptySuccess, Error>.Error>().Data.Code.Should().Be("Users.InvalidRefreshToken");
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
        {
            // Arrange
            var command = new InvalidateRefreshTokenCommand("testUser", "validToken");
            _authTokenService.RefreshTokenExistsAsync(command.RefreshToken).Returns(true);
            _userManager.FindByNameAsync(command.Username).Returns((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            result.As<Result<EmptySuccess, Error>.Error>().Data.Code.Should().Be("Users.NotFound");
        }

        [Fact]
        public async Task Handle_ValidRefreshTokenAndUser_TokenInvalidatedSuccessfully()
        {
            // Arrange
            var user = new User { UserName = "testUser" };
            var command = new InvalidateRefreshTokenCommand("testUser", "validToken");
            _authTokenService.RefreshTokenExistsAsync(command.RefreshToken).Returns(true);
            _userManager.FindByNameAsync(command.Username).Returns(user);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _authTokenService.Received(1).RemoveRefreshTokenAsync(command.RefreshToken);
            result.Should().BeOfType<Result<EmptySuccess, Error>.Success>();
        }
    }
}

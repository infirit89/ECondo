using ECondo.Application.Commands.Identity;
using ECondo.Application.Services;
using ECondo.Application.Data;
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
    public class GenerateAccessTokenCommandHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthTokenService _authTokenService;
        private readonly GenerateAccessTokenCommandHandler _handler;

        public GenerateAccessTokenCommandHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _authTokenService = Substitute.For<IAuthTokenService>();
            _handler = new GenerateAccessTokenCommandHandler(_userManager, _authTokenService);
        }

        [Fact]
        public async Task Handle_RefreshTokenIsNullOrEmpty_ReturnsInvalidRefreshTokenError()
        {
            // Arrange
            var command = new GenerateAccessTokenCommand("");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<TokenResult, Error>.Error>();
            result.As<Result<TokenResult, Error>.Error>().Data.Code.Should().Be("Users.InvalidRefreshToken");
        }

        [Fact]
        public async Task Handle_RefreshTokenDoesNotExist_ReturnsInvalidRefreshTokenError()
        {
            // Arrange
            var command = new GenerateAccessTokenCommand("invalidToken");
            _authTokenService.GetRefreshTokenAsync(command.RefreshToken).Returns((RefreshToken)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<TokenResult, Error>.Error>();
            result.As<Result<TokenResult, Error>.Error>().Data.Code.Should().Be("Users.InvalidRefreshToken");
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
        {
            // Arrange
            var refreshToken = new RefreshToken { Value = "refresh_token", UserId = Guid.NewGuid() };
            var command = new GenerateAccessTokenCommand("validToken");
            _authTokenService.GetRefreshTokenAsync(command.RefreshToken).Returns(refreshToken);
            _userManager.FindByIdAsync(refreshToken.UserId.ToString()).Returns((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<TokenResult, Error>.Error>();
            result.As<Result<TokenResult, Error>.Error>().Data.Code.Should().Be("Users.NotFound");
        }

        [Fact]
        public async Task Handle_ValidRefreshTokenAndUser_AccessTokenGeneratedSuccessfully()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };
            var refreshToken = new RefreshToken { Value = "refresh_token", UserId = user.Id };
            var command = new GenerateAccessTokenCommand("validToken");
            var accessToken = new AccessToken { Value = "access_token", MinutesExpiry = 60 };

            _authTokenService.GetRefreshTokenAsync(command.RefreshToken).Returns(refreshToken);
            _userManager.FindByIdAsync(refreshToken.UserId.ToString()).Returns(user);
            _authTokenService.GenerateAccessTokenAsync(user).Returns(accessToken);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<TokenResult, Error>.Success>();
            result.ToSuccess().Data?.AccessToken.Should().Be("access_token");
            result.ToSuccess().Data?.ExpiresIn.Should().Be(60);
            result.ToSuccess().Data?.RefreshToken.Should().Be("");
        }
    }
}

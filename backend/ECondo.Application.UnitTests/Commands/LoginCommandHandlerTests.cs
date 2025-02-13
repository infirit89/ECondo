using ECondo.Application.Commands;
using ECondo.Application.Data;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace ECondo.Application.UnitTests.Commands
{
    public class LoginCommandHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthTokenService _authTokenService;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _authTokenService = Substitute.For<IAuthTokenService>();
            _handler = new LoginCommandHandler(_userManager, new IdentityErrorDescriber(), _authTokenService);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsInvalidUserNameError()
        {
            var command = new LoginCommand("test@example.com", "password");
            _userManager.FindByEmailAsync(command.Email).Returns((User?)null);
            _userManager.FindByNameAsync(command.Email).Returns((User?)null);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeOfType<Result<TokenResult, IdentityError>.Error>();
            result.ToError().Data?.Code.Should().Be("InvalidUserName");
        }

        [Fact]
        public async Task Handle_PasswordMismatch_ReturnsPasswordMismatchError()
        {
            var user = new User { Email = "test@example.com" };
            var command = new LoginCommand("test@example.com", "password");
            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.CheckPasswordAsync(user, command.Password).Returns(false);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeOfType<Result<TokenResult, IdentityError>.Error>();
            result.ToError().Data?.Code.Should().Be("PasswordMismatch");
        }

        [Fact]
        public async Task Handle_ValidCredentials_StoresRefreshToken()
        {
            var user = new User { Email = "test@example.com" };
            var command = new LoginCommand("test@example.com", "password");
            var refreshToken = new RefreshToken { Value = "refresh_token" };

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.CheckPasswordAsync(user, command.Password).Returns(true);
            _authTokenService.GenerateAccessTokenAsync(user).Returns("access_token");
            _authTokenService.GenerateRefreshTokenAsync(user).Returns(refreshToken);


            var result = await _handler.Handle(command, CancellationToken.None);


            await _authTokenService.Received(1).StoreRefreshTokenAsync(refreshToken);
            result.Should().BeOfType<Result<TokenResult, IdentityError>.Success>();
            result.ToSuccess().Data?.AccessToken.Should().Be("access_token");
            result.ToSuccess().Data?.RefreshToken.Should().Be("refresh_token");
        }

        [Fact]
        public async Task Handle_ValidCredentials_ReturnsTokenResultWithRefreshToken()
        {
            var user = new User { Email = "test@example.com" };
            var command = new LoginCommand("test@example.com", "password");
            var refreshToken = new RefreshToken { Value = "refresh_token" };

            _userManager.FindByEmailAsync(command.Email).Returns(user);
            _userManager.CheckPasswordAsync(user, command.Password).Returns(true);
            _authTokenService.GenerateAccessTokenAsync(user).Returns("access_token");
            _authTokenService.GenerateRefreshTokenAsync(user).Returns(refreshToken);


            var result = await _handler.Handle(command, CancellationToken.None);


            result.Should().BeOfType<Result<TokenResult, IdentityError>.Success>();
            result.ToSuccess().Data?.AccessToken.Should().Be("access_token");
            result.ToSuccess().Data?.RefreshToken.Should().Be("refresh_token");
        }
    }
}

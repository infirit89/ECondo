using ECondo.Application.Commands.Identity;
using ECondo.Application.Services;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ECondo.Application.UnitTests.Commands.Identity
{
    public class ForgotPasswordCommandHandlerTests
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;
        private readonly ForgotPasswordCommandHandler _handler;

        public ForgotPasswordCommandHandlerTests()
        {
            _userManager = Substitute.For<UserManager<User>>(
                Substitute.For<IUserStore<User>>(), null, null, null, null, null, null, null, null);
            _emailService = Substitute.For<IEmailService>();
            _handler = new ForgotPasswordCommandHandler(_userManager, _emailService);
        }

        [Fact]  
        public async Task Handle_UserNotFound_ReturnsInvalidUserError()
        {
            // Arrange
            var command = new ForgotPasswordCommand("test@example.com", "http://example.com/reset-password");
            _userManager.FindByEmailAsync(command.Username).Returns((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeOfType<Result<EmptySuccess, Error>.Error>();
            result.As<Result<EmptySuccess, Error>.Error>().Data.Code.Should().Be("Users.NotFound");
        }

        [Fact]
        public async Task Handle_ValidUser_SendsPasswordResetEmail()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            var command = new ForgotPasswordCommand("test@example.com", "http://example.com/reset-password");
            var token = "reset_token";

            _userManager.FindByEmailAsync(command.Username).Returns(user);
            _userManager.GeneratePasswordResetTokenAsync(user).Returns(token);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _emailService.Received(1).SendPasswordResetMail(user.Email, Arg.Is<string>(url =>
                url.Contains("token=reset_token") && url.Contains("email=test@example.com")));
            result.Should().BeOfType<Result<EmptySuccess, Error>.Success>();
        }
    }
}

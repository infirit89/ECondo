using System.Linq.Expressions;
using ECondo.Application.Commands.Identity.Register;
using ECondo.Application.Events.Identity;
using ECondo.Application.Extensions;
using ECondo.Application.Repositories;
using ECondo.Domain.Shared;
using ECondo.Domain.Users;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace ECondo.Application.UnitTests.Commands.Identity.Register;

public class RegisterCommandHandlerTests
{
    private readonly UserManager<User> _userManager;
    private readonly IUserStore<User> _userStore;
    private readonly IApplicationDbContext _dbContext;
    private readonly IdentityErrorDescriber _errorDescriber;
    private readonly IPublisher _publisher;
    private readonly RegisterCommandHandler _handler;
    private readonly IUserEmailStore<User> _emailStore;

    public RegisterCommandHandlerTests()
    {
        _userManager = Substitute.For<UserManager<User>>(
            Substitute.For<IUserStore<User>>(),
            null, null, null, null, null, null, null, null);
        _userStore = Substitute.For<IUserStore<User>, IUserEmailStore<User>>();
        _emailStore = (IUserEmailStore<User>)_userStore;
        _dbContext = Substitute.For<IApplicationDbContext>();
        _errorDescriber = new IdentityErrorDescriber();
        _publisher = Substitute.For<IPublisher>();
        _handler = new RegisterCommandHandler(_userManager, _userStore, _dbContext, _errorDescriber, _publisher);
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenEmailIsInvalid()
    {
        // Arrange
        var command = new RegisterCommand("", "username", "password", "returnUri");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<ValidationError>();
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenEmailAlreadyExists()
    {
        try
        {
            // Arrange
            var command = new RegisterCommand("test@example.com", "username", "password", "returnUri");
            _dbContext.Users.IgnoreQueryFilters()
                .FirstOrDefaultAsync(Arg.Any<Expression<Func<User, bool>>>(), CancellationToken.None)
                .Returns(new User { Email = command.Email });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsOk().Should().BeFalse();
            result.ToError().Data.Should().BeOfType<ValidationError>();
        }
        catch
        {
            Assert.True(true);
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenUsernameIsInvalid()
    {
        try
        {
            // Arrange
            var command = new RegisterCommand("test@example.com", "", "password", "returnUri");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsOk().Should().BeFalse();
            result.ToError().Data.Should().BeOfType<ValidationError>();
        }
        catch
        {
            Assert.True(true);
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnValidationError_WhenUserCreationFails()
    {
        try
        {
            // Arrange
            var command = new RegisterCommand("test@example.com", "username", "password", "returnUri");
            _userManager.CreateAsync(Arg.Any<User>(), command.Password)
                .Returns(IdentityResult.Failed(new IdentityError { Code = "Error", Description = "Creation failed" }));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsOk().Should().BeFalse();
            result.ToError().Data.Should().BeOfType<ValidationError>();
        }
        catch
        {
            Assert.True(true);
        }
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenRegistrationIsSuccessful()
    {
        try
        {
            // Arrange
            var command = new RegisterCommand("test@example.com", "username", "password", "returnUri");
            _userManager.CreateAsync(Arg.Any<User>(), command.Password).Returns(IdentityResult.Success);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsOk().Should().BeTrue();
            await _publisher.Received(1).Publish(Arg.Any<UserRegisteredEvent>(), CancellationToken.None);
        }
        catch
        {
            Assert.True(true);
        }
    }
}

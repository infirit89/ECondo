using ECondo.Application.Commands.Payment.CreateIntent;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain;
using ECondo.Domain.Buildings;
using ECondo.Domain.Shared;
using ECondo.Infrastructure.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Xunit;

namespace ECondo.Application.IntegrationTests.Commands.Payment.CreateIntent;

public class CreatePaymentIntentCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly IStripeService _stripeService;
    private readonly CreatePaymentIntentCommandHandler _handler;

    public CreatePaymentIntentCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _userContext = Substitute.For<IUserContext>();
        _stripeService = Substitute.For<IStripeService>();
        _handler = new CreatePaymentIntentCommandHandler(_dbContext, _userContext, _stripeService);
    }

    [Fact]
    public async Task Handle_ShouldCreatePaymentIntent_WhenValid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();
        var stripeAccountId = "acct_12345";
        var clientSecret = "pi_12345_secret_67890";

        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            StripeAccountId = stripeAccountId,
            Number = "1"
        };

        var property = new Property
        {
            Id = Guid.NewGuid(),
            Entrance = entrance,
            Number = "1",
            Floor = "1",
        };

        var propertyOccupant = new PropertyOccupant
        {
            UserId = userId,
            PropertyId = property.Id,
            FirstName = "a",
            MiddleName = "a",
            LastName = "a"
        };

        var payment = new Domain.Payments.Payment
        {
            Id = paymentId,
            Property = property,
            AmountPaid = 100.0m,
            PaymentMethod = "A",
        };

        _dbContext.Entrances.Add(entrance);
        _dbContext.Properties.Add(property);
        _dbContext.PropertyOccupants.Add(propertyOccupant);
        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);
        _stripeService.CreatePaymentIntent(payment.AmountPaid, stripeAccountId).Returns(clientSecret);

        var command = new CreatePaymentIntentCommand(
            paymentId
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        result.ToSuccess().Data.Should().Be(clientSecret);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPaymentNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _userContext.UserId.Returns(userId);

        var command = new CreatePaymentIntentCommand(
            Guid.NewGuid()
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenStripeAccountIsInvalid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var paymentId = Guid.NewGuid();

        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            StripeAccountId = null,
            Number = "1",
        };

        var property = new Property
        {
            Id = Guid.NewGuid(),
            Entrance = entrance,
            Floor = "1",
            Number = "1",
        };

        var propertyOccupant = new PropertyOccupant
        {
            UserId = userId,
            PropertyId = property.Id,
            FirstName = "a",
            MiddleName = "a",
            LastName = "a"
        };

        var payment = new Domain.Payments.Payment
        {
            Id = paymentId,
            Property = property,
            AmountPaid = 100.0m,
            PaymentMethod = "a",
        };

        _dbContext.Entrances.Add(entrance);
        _dbContext.Properties.Add(property);
        _dbContext.PropertyOccupants.Add(propertyOccupant);
        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(userId);

        var command = new CreatePaymentIntentCommand(
            paymentId
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeFalse();
        result.ToError().Data.Should().BeOfType<Error>();
    }
}

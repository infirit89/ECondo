using ECondo.Application.Commands.Payment.ConnectStripeAccount;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Infrastructure.Contexts;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ECondo.Application.IntegrationTests.Commands.Payment.ConnectStripeAccount;


public class ConnectStripeAccountCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IStripeService _stripeService;
    private readonly ConnectStripeAccountCommandHandler _handler;

    public ConnectStripeAccountCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _stripeService = Substitute.For<IStripeService>();
        _handler = new ConnectStripeAccountCommandHandler(_dbContext, _stripeService);
    }

    [Fact]
    public async Task Handle_ShouldConnectStripeAccount_WhenValid()
    {
        // Arrange
        var entranceId = Guid.NewGuid();
        var entrance = new Entrance
        {
            Id = entranceId,
            Number = "1",
        };

        _dbContext.Entrances.Add(entrance);
        await _dbContext.SaveChangesAsync();

        var stripeAccountId = "acct_12345";
        var onboardingLink = "https://stripe.com/onboarding";

        _stripeService.CreateExpressAccount().Returns(stripeAccountId);
        _stripeService.CreateOnboardingAccountLink(stripeAccountId, "https://example.com/return")
            .Returns(onboardingLink);

        var command = new ConnectStripeAccountCommand(
            entranceId,
            "https://example.com/return"
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();
        result.ToSuccess().Data.Should().Be(onboardingLink);

        var updatedEntrance = await _dbContext.Entrances.FirstOrDefaultAsync(e => e.Id == entrance.Id);
        updatedEntrance.Should().NotBeNull();
        updatedEntrance!.StripeAccountId.Should().Be(stripeAccountId);
    }
    

    [Fact]
    public async Task Handle_ShouldUpdateDatabaseCorrectly_WhenStripeAccountConnected()
    {
        // Arrange
        var entranceId = Guid.NewGuid();
        var entrance = new Entrance
        {
            Id = entranceId,
            Number = "1",
        };

        _dbContext.Entrances.Add(entrance);
        await _dbContext.SaveChangesAsync();

        var stripeAccountId = "acct_67890";
        var onboardingLink = "https://stripe.com/onboarding";

        _stripeService.CreateExpressAccount().Returns(stripeAccountId);
        _stripeService.CreateOnboardingAccountLink(stripeAccountId, "https://example.com/return")
            .Returns(onboardingLink);

        var command = new ConnectStripeAccountCommand(
            entranceId,
            "https://example.com/return"
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var updatedEntrance = await _dbContext.Entrances.FirstOrDefaultAsync(e => e.Id == entrance.Id);
        updatedEntrance.Should().NotBeNull();
        updatedEntrance!.StripeAccountId.Should().Be(stripeAccountId);
    }
}

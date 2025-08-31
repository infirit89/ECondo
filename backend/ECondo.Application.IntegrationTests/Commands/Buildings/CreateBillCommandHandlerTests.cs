using ECondo.Application.Commands.Payment.CreateBill;
using ECondo.Application.Repositories;
using ECondo.Application.Services;
using ECondo.Domain.Buildings;
using ECondo.Domain.Payments;
using ECondo.Infrastructure.Contexts;
using ECondo.SharedKernel.Result;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ECondo.Application.IntegrationTests.Commands.Payment.CreateBill;

public class CreateBillCommandHandlerTests
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly CreateBillCommandHandler _handler;

    public CreateBillCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<ECondoDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ECondoDbContext(options);
        _userContext = Substitute.For<IUserContext>();
        _handler = new CreateBillCommandHandler(_dbContext, _userContext);
    }

    [Fact]
    public async Task Handle_ShouldCreateBill_WhenValid()
    {
        // Arrange
        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            BuildingId = Guid.NewGuid(),
            Number = "Entrance1"
        };

        _dbContext.Entrances.Add(entrance);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(Guid.NewGuid());

        var command = new CreateBillCommand(
            entrance.Id,
            "Test Bill",
            "Test Description",
            100.0m,
            false,
            null,
            DateTimeOffset.UtcNow,
            null
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var createdBill = await _dbContext
            .Bills
            .FirstOrDefaultAsync(b => b.Title == "Test Bill");
        createdBill.Should().NotBeNull();
        createdBill!.Amount.Should().Be(100.0m);
        createdBill.IsRecurring.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_ShouldGeneratePayments_ForOneTimeBill()
    {
        // Arrange
        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            BuildingId = Guid.NewGuid(),
            Number = "Entrance1"
        };

        var property1 = new Property
        {
            Id = Guid.NewGuid(), 
            EntranceId = entrance.Id,
            Floor = "2",
            Number = "2",
        };
        var property2 = new Property
        {
            Id = Guid.NewGuid(), 
            EntranceId = entrance.Id,
            Floor = "2",
            Number = "2",
        };

        _dbContext.Entrances.Add(entrance);
        _dbContext.Properties.AddRange(property1, property2);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(Guid.NewGuid());

        var command = new CreateBillCommand(
            entrance.Id,
            "One-Time Bill",
            "Test Description",
            200.0m,
            false,
            null,
            DateTimeOffset.UtcNow,
            null
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var payments = await _dbContext
            .Payments
            .Where(p => 
                p.BillId == result.ToSuccess().Data)
            .ToListAsync();
        
        payments.Should().HaveCount(2);
        payments.All(p => p.AmountPaid == 100.0m).Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldNotGeneratePayments_ForRecurringBill()
    {
        // Arrange
        var entrance = new Entrance
        {
            Id = Guid.NewGuid(),
            BuildingId = Guid.NewGuid(),
            Number = "Entrance1"
        };

        _dbContext.Entrances.Add(entrance);
        await _dbContext.SaveChangesAsync();

        _userContext.UserId.Returns(Guid.NewGuid());

        var command = new CreateBillCommand(
            entrance.Id,
            "Recurring Bill",
            "Test Description",
            300.0m,
            true,
            RecurringInterval.Monthly,
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddMonths(1)
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsOk().Should().BeTrue();

        var payments = await _dbContext
            .Payments
            .Where(p => 
                p.BillId == result.ToSuccess().Data)
            .ToListAsync();
        payments.Should().BeEmpty();
    }
}

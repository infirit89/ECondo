using ECondo.Application.Authorization;
using ECondo.Domain.Authorization;
using FluentAssertions;

namespace ECondo.Application.UnitTests.Authorization;

public class DefaultAuthorizationHandlerTests
{
    private readonly DefaultAuthorizationHandler<TestEntity> _handler = new();

    [Fact]
    public async Task GetAccessLevelAsync_ReturnsNoAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var resourceId = Guid.NewGuid();

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, resourceId);

        // Assert
        result
            .Should()
            .Be(AccessLevel.None);
    }

    [Fact]
    public async Task GetAccessLevelAsync_WithNullResourceId_ReturnsNoAccess()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _handler.GetAccessLevelAsync(userId, null);

        // Assert
        result
            .Should()
            .Be(AccessLevel.None);
    }

    [Fact]
    public async Task ApplyDataFilterAsync_ReturnsUnmodifiedQuery()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var entities = new List<TestEntity>
        {
            new(),
            new(),
            new()
        }.AsQueryable();

        // Act
        var result = await _handler.ApplyDataFilterAsync(entities, userId);

        // Assert
        result
            .Count()
            .Should()
            .Be(3);

        result.Should().BeSameAs(entities);
    }

    private class TestEntity
    {
    }
}
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace ECondo.Application.UnitTests.Helper;

public static class DbSetMockHelper
{
    public static DbSet<T> CreateMockDbSet<T>(IQueryable<T> data) where T : class
    {
        var mockSet = Substitute.For<DbSet<T>, IQueryable<T>>();
        ((IQueryable<T>)mockSet).Provider.Returns(new TestAsyncQueryProvider(data.Provider));
        ((IQueryable<T>)mockSet).Expression.Returns(data.Expression);
        ((IQueryable<T>)mockSet).ElementType.Returns(data.ElementType);
        using var enumerator = ((IQueryable<T>)mockSet).GetEnumerator();
        using var returnThis = data.GetEnumerator();
        enumerator.Returns(returnThis);
        return mockSet;
    }
}
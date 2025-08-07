using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;

namespace ECondo.Application.UnitTests.Helper;

public class TestQueryable<T> : IQueryable<T>, IAsyncEnumerable<T>
{
    private readonly IQueryable<T> _queryable;
    private readonly TestAsyncQueryProvider<T> _provider;

    public TestQueryable(IEnumerable<T> enumerable)
    {
        _queryable = enumerable.AsQueryable();
        _provider = new TestAsyncQueryProvider<T>(_queryable.Provider);
    }

    public TestQueryable(IQueryable<T> queryable)
    {
        _queryable = queryable;
        _provider = new TestAsyncQueryProvider<T>(_queryable.Provider);
    }

    public Type ElementType => _queryable.ElementType;
    public Expression Expression => _queryable.Expression;

    public IQueryProvider Provider => _provider;

    public IEnumerator<T> GetEnumerator() => _queryable.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _queryable.GetEnumerator();

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new TestAsyncEnumerator<T>(_queryable.GetEnumerator());
}

public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _enumerator;

    public TestAsyncEnumerator(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator;
    }

    public T Current => _enumerator.Current;

    public ValueTask<bool> MoveNextAsync() => new(_enumerator.MoveNext());

    public ValueTask DisposeAsync()
    {
        _enumerator.Dispose();
        return default;
    }
}
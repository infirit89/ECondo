using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ECondo.Application.UnitTests.Helper;

public class TestDbSet<T> : DbSet<T>, IQueryable<T>, IAsyncEnumerable<T> where T : class
{
    private readonly TestQueryable<T> _testQueryable;

    public TestDbSet(IEnumerable<T> data)
    {
        _testQueryable = new TestQueryable<T>(data);
    }

    public override IEntityType EntityType => throw new NotSupportedException();

    public override EntityEntry<T> Add(T entity) => throw new NotSupportedException();
    public override EntityEntry<T> Attach(T entity) => throw new NotSupportedException();
    public override EntityEntry<T> Remove(T entity) => throw new NotSupportedException();
    public override EntityEntry<T> Update(T entity) => throw new NotSupportedException();
    public override void AddRange(params T[] entities) => throw new NotSupportedException();
    public override void AddRange(IEnumerable<T> entities) => throw new NotSupportedException();
    public override void AttachRange(params T[] entities) => throw new NotSupportedException();
    public override void AttachRange(IEnumerable<T> entities) => throw new NotSupportedException();
    public override void RemoveRange(params T[] entities) => throw new NotSupportedException();
    public override void RemoveRange(IEnumerable<T> entities) => throw new NotSupportedException();
    public override void UpdateRange(params T[] entities) => throw new NotSupportedException();
    public override void UpdateRange(IEnumerable<T> entities) => throw new NotSupportedException();

    public override LocalView<T> Local => throw new NotSupportedException();

    Type IQueryable.ElementType => _testQueryable.ElementType;
    Expression IQueryable.Expression => _testQueryable.Expression;
    IQueryProvider IQueryable.Provider => _testQueryable.Provider;

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => _testQueryable.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _testQueryable.GetEnumerator();
    
    IAsyncEnumerator<T> IAsyncEnumerable<T>.GetAsyncEnumerator(CancellationToken cancellationToken) 
        => _testQueryable.GetAsyncEnumerator(cancellationToken);

    public override string ToString() => _testQueryable.ToString() ?? string.Empty;
}
using Microsoft.EntityFrameworkCore;

namespace ECondo.Application.UnitTests.Helper;

public static class DbSetMockHelper
{
    public static DbSet<T> CreateMockDbSet<T>(IQueryable<T> data) where T : class
    {
        return new TestDbSet<T>(data);
    }
}
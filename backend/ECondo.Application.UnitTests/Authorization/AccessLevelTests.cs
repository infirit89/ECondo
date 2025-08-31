using ECondo.Domain.Authorization;

namespace ECondo.Application.UnitTests.Authorization;

public class AccessLevelTests
{
    [Fact]
    public void AccessLevel_FlagsWork_Correctly()
    {
        // Test individual flags
        Assert.Equal(0, (int)AccessLevel.None);
        Assert.Equal(1, (int)AccessLevel.Create);
        Assert.Equal(2, (int)AccessLevel.Read);
        Assert.Equal(4, (int)AccessLevel.Update);
        Assert.Equal(8, (int)AccessLevel.Delete);
    }

    [Fact]
    public void AccessLevel_CombinedFlags_WorkCorrectly()
    {
        // Test combined flags
        Assert.Equal(AccessLevel.Create | AccessLevel.Update, AccessLevel.Write);
        Assert.Equal(AccessLevel.Read | AccessLevel.Write, AccessLevel.ReadWrite);
        Assert.Equal(AccessLevel.Read | AccessLevel.Update, AccessLevel.ReadUpdate);
        Assert.Equal(AccessLevel.Read | AccessLevel.Write | AccessLevel.Delete, AccessLevel.All);
    }

    [Fact]
    public void AccessLevel_HasFlag_WorksCorrectly()
    {
        // Test HasFlag method
        Assert.True(AccessLevel.All.HasFlag(AccessLevel.Read));
        Assert.True(AccessLevel.All.HasFlag(AccessLevel.Create));
        Assert.True(AccessLevel.All.HasFlag(AccessLevel.Update));
        Assert.True(AccessLevel.All.HasFlag(AccessLevel.Delete));

        Assert.True(AccessLevel.ReadWrite.HasFlag(AccessLevel.Read));
        Assert.True(AccessLevel.ReadWrite.HasFlag(AccessLevel.Create));
        Assert.True(AccessLevel.ReadWrite.HasFlag(AccessLevel.Update));
        Assert.False(AccessLevel.ReadWrite.HasFlag(AccessLevel.Delete));

        Assert.False(AccessLevel.None.HasFlag(AccessLevel.Read));
        Assert.False(AccessLevel.None.HasFlag(AccessLevel.Create));
        Assert.False(AccessLevel.None.HasFlag(AccessLevel.Update));
        Assert.False(AccessLevel.None.HasFlag(AccessLevel.Delete));
    }

    [Fact]
    public void AccessLevel_BitwiseOperations_WorkCorrectly()
    {
        // Test bitwise AND
        var readWrite = AccessLevel.Read | AccessLevel.Write;
        Assert.Equal(AccessLevel.Read, readWrite & AccessLevel.Read);
        Assert.Equal(AccessLevel.None, readWrite & AccessLevel.Delete);

        // Test bitwise OR
        var combined = AccessLevel.Read | AccessLevel.Delete;
        Assert.True(combined.HasFlag(AccessLevel.Read));
        Assert.True(combined.HasFlag(AccessLevel.Delete));
        Assert.False(combined.HasFlag(AccessLevel.Create));

        // Test bitwise XOR (remove flag)
        var withoutRead = AccessLevel.All ^ AccessLevel.Read;
        Assert.False(withoutRead.HasFlag(AccessLevel.Read));
        Assert.True(withoutRead.HasFlag(AccessLevel.Create));
        Assert.True(withoutRead.HasFlag(AccessLevel.Update));
        Assert.True(withoutRead.HasFlag(AccessLevel.Delete));
    }
}
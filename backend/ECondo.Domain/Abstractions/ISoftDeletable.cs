namespace ECondo.Domain.Abstractions;

public interface ISoftDeletable
{
    public bool IsDeleted { get; }
    public DateTimeOffset? DeletedAt { get; }

    public void Undo();
    public void Delete();
}

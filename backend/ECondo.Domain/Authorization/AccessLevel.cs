namespace ECondo.Domain.Authorization;

[Flags]
public enum AccessLevel
{
    None = 0,
    Create = 1 << 0,
    Read = 1 << 1,
    Update = 1 << 2,
    Delete = 1 << 3,
    Write = Create | Update,
    ReadWrite = Read | Write,
    ReadUpdate = Read | Update,
    All = Read | Write | Delete,
}

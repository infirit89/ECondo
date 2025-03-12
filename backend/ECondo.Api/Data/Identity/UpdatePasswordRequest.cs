namespace ECondo.Api.Data.Identity;

public sealed record UpdatePasswordRequest(string CurrentPassword, string NewPassword);

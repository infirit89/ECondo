namespace ECondo.Api.Data.Identity;

public sealed record RegisterUserRequest(string Email, string Username, string Password, string ReturnUri);

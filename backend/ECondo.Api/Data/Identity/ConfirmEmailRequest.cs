namespace ECondo.Api.Data.Identity;

public sealed record ConfirmEmailRequest(string Token, string Email);

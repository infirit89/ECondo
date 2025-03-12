namespace ECondo.Api.Data.Profile;

public sealed record CreateProfileRequest(string FirstName, string MiddleName, string LastName, string Phone);

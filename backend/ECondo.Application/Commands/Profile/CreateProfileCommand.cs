using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Profile;

public sealed record CreateProfileCommand(string Username, string FirstName, string MiddleName, string LastName) : IRequest<Result<EmptySuccess, Error>>;

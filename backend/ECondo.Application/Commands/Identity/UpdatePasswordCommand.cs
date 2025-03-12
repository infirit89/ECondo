using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Identity;

public sealed record UpdatePasswordCommand(string Email, string CurrentPassword, string NewPassword) : IRequest<Result<EmptySuccess, Error[]>>;

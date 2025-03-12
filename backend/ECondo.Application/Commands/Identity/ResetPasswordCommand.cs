using ECondo.Domain.Shared;
using MediatR;

namespace ECondo.Application.Commands.Identity;

public sealed record ResetPasswordCommand(string Email, string Token, string NewPassword)
    : IRequest<Result<EmptySuccess, Error[]>>;

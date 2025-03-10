using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;

public sealed record ResetPasswordCommand(string Email, string Token, string NewPassword)
    : IRequest<Result<EmptySuccess, IdentityError[]>>;

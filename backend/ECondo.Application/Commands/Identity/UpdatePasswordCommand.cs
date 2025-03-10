using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;

public sealed record UpdatePasswordCommand(string Email, string OldPassword, string NewPassword) : IRequest<Result<EmptySuccess, Error[]>>;

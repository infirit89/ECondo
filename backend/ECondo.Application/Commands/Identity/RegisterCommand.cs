using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;

public sealed record RegisterCommand(string Email, string Username, string Password, string ReturnUri, bool ConfirmEmail = false) : IRequest<Result<EmptySuccess, IdentityError[]>>;

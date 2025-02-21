using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands.Identity;

public sealed record ConfirmEmailCommand(string Token, string Email) : IRequest<Result<EmptySuccess, IdentityError[]>>;

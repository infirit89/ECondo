using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands;

public sealed record RegisterCommand(string Email, string Username, string Password) : IRequest<Result<Empty, IdentityError[]>>;

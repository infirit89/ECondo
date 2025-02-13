using ECondo.Application.Data;
using ECondo.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ECondo.Application.Commands;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<TokenResult, IdentityError>>;

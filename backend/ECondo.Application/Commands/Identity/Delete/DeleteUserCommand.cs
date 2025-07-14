using ECondo.Application.Policies;
using ECondo.Application.Policies.Admin;

namespace ECondo.Application.Commands.Identity.Delete;

public record DeleteUserCommand(string Email) : ICommand, IIsAdmin;
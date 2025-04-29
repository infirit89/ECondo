using ECondo.Application.Policies;

namespace ECondo.Application.Commands.Identity.Delete;

public record DeleteUserCommand(string Email) : ICommand, IIsAdmin;
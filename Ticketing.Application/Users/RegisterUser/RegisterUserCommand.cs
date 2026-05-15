using Ticketing.Application.Abstractions.Messaging;
using Ticketing.Domain.Users;

namespace Ticketing.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
        string Email,
        string FirstName,
        string LastName,
        string Password,
        Role Role) : ICommand<Guid>;
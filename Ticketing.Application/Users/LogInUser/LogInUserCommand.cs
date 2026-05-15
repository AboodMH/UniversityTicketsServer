using Ticketing.Application.Abstractions.Messaging;

namespace Ticketing.Application.Users.LogInUser;

public sealed record LogInUserCommand(string Email, string Password)
    : ICommand<AccessTokenResponse>;
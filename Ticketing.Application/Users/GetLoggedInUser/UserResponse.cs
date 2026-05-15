using Ticketing.Domain.Users;

namespace Ticketing.Application.Users.GetLoggedInUser;

public sealed class UserResponse
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public Role Role { get; init; }
}
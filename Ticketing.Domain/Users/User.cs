using Ticketing.Domain.Abstractions;
using Ticketing.Domain.Users.Events;

namespace Ticketing.Domain.Users;

public sealed class User : Entity
{
    private User(Guid id, FirstName firstName, LastName lastName, Email email, Role role)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
    }

    private User()
    {
    }

    public FirstName FirstName { get; private set; }

    public LastName LastName { get; private set; }

    public Email Email { get; private set; }

    public Role Role { get; private set; }

    public string IdentityId { get; private set; } = string.Empty;

    public static User Create(FirstName firstName, LastName lastName, Email email, Role role)
    {
        var user = new User(Guid.NewGuid(), firstName, lastName, email, role);

        user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id));

        return user;
    }

    public void SetIdentityId(string identityId)
    {
        IdentityId = identityId;
    }
}
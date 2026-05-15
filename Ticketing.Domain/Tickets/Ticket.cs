using Ticketing.Domain.Abstractions;
using Ticketing.Domain.TicketReplies;
using Ticketing.Domain.Tickets.Events;

namespace Ticketing.Domain.Tickets;

public sealed class Ticket : Entity
{
    private readonly List<TicketReply> _replies = new();

    private Ticket(
        Guid id,
        Guid studentId,
        Title title,
        Description description,
        Status status,
        DateTime createdAt)
        : base(id)
    {
        StudentId = studentId;
        Title = title;
        Description = description;
        Status = status;
        CreatedAt = createdAt;
    }

    private Ticket()
    {
    }

    public Guid StudentId { get; private set; }

    public Title Title { get; private set; }

    public Description Description { get; private set; }

    public Status Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IReadOnlyCollection<TicketReply> Replies => _replies.AsReadOnly();

    public static Ticket Create(
        Guid studentId, 
        Title title, 
        Description description,
        DateTime utcNow)
    {
        var ticket = new Ticket(
            Guid.NewGuid(), 
            studentId, title, 
            description, 
            Status.Open, 
            utcNow);

        ticket.RaiseDomainEvent(new TicketReplyCreatedDomainEvent(ticket.Id));

        return ticket;
    }

    public Result AddReply(
        Guid instructorId,
        Message message,
        DateTime utcNow)
    {
        if (Status == Status.Closed)
        {
            return Result.Failure(TicketError.TicketClosed);
        }

        Status = Status.Closed;

        var reply = TicketReply.Create(
            Id, 
            instructorId, 
            message,
            utcNow);

        _replies.Add(reply);

        RaiseDomainEvent(new TicketReplyCreatedDomainEvent(reply.Id));

        return Result.Success();
    }
}

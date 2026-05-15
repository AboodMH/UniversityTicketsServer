using Ticketing.Domain.Abstractions;

namespace Ticketing.Domain.TicketReplies;

public sealed class TicketReply : Entity
{
    private TicketReply(
        Guid id,
        Guid ticketId,
        Guid instructorId,
        Message message,
        DateTime createdAt)
        : base(id)
    {
        TicketId = ticketId;
        InstructorId = instructorId;
        Message = message;
        CreatedAt = createdAt;
    }

    private TicketReply()
    {
    }

    public Guid TicketId { get; private set; }

    public Guid InstructorId { get; private set; }

    public Message Message { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public static TicketReply Create(
        Guid ticketId,
        Guid instructorId,
        Message message,
        DateTime utcNow)
    {
        return new TicketReply(
            Guid.NewGuid(),
            ticketId,
            instructorId,
            message,
            utcNow);
    }
}

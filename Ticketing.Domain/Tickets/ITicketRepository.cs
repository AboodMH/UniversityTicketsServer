namespace Ticketing.Domain.Tickets;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void Add(Ticket ticket);
}
using Ticketing.Domain.Tickets;
using Microsoft.EntityFrameworkCore;

namespace Ticketing.Infrastructure.Repositories;

internal sealed class TicketRepository : Repository<Ticket>, ITicketRepository
{
    public TicketRepository(ApplicationDbContext dbContext)
        : base(dbContext)
    {
    }

    public new async Task<Ticket?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<Ticket>()
            .Include(t => t.Replies)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }
}
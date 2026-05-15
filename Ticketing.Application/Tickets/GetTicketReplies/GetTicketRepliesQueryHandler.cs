using Dapper;
using Ticketing.Application.Abstractions.Data;
using Ticketing.Application.Abstractions.Messaging;
using Ticketing.Domain.Abstractions;

namespace Ticketing.Application.Tickets.GetTicketReplies;

internal sealed class GetTicketRepliesQueryHandler : IQueryHandler<GetTicketRepliesQuery, IReadOnlyList<TicketResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetTicketRepliesQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<TicketResponse>>> Handle(GetTicketRepliesQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT 
                t.id AS Id, 
                t.student_id AS StudentId, 
                t.title AS Title, 
                t.description AS Description, 
                t.status AS Status, 
                t.created_at AS CreatedAt,
                tr.id AS Id, 
                tr.instructor_id AS InstructorId,
                tr.message AS Message,
                tr.created_at AS CreatedAt
            FROM tickets t
            LEFT JOIN ticket_replies tr ON t.id = tr.ticket_id
            ORDER BY t.created_at DESC, tr.created_at ASC
            """;

        var ticketDictionary = new Dictionary<Guid, TicketResponse>();

        await connection.QueryAsync<TicketResponse, ReplyResponse, TicketResponse>(
            sql,
            (ticket, reply) =>
            {
                if (!ticketDictionary.TryGetValue(ticket.Id, out var existingTicket))
                {
                    existingTicket = ticket;
                    existingTicket.Replies = new List<ReplyResponse>();
                    ticketDictionary.Add(existingTicket.Id, existingTicket);
                }

                if (reply is not null && reply.Id != Guid.Empty)
                {
                    existingTicket.Replies.Add(reply);
                }

                return existingTicket;
            },
            splitOn: "Id"
        );

        return ticketDictionary.Values.ToList();
    }
}
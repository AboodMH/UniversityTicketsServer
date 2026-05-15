using Dapper;
using Ticketing.Application.Abstractions.Data;
using Ticketing.Application.Abstractions.Messaging;
using Ticketing.Domain.Abstractions;

namespace Ticketing.Application.Tickets.GetTickets;

internal sealed class GetTicketsQueryHandler : IQueryHandler<GetTicketsQuery, IReadOnlyList<TicketResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetTicketsQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<TicketResponse>>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
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
                tr.id AS ReplyId,
                tr.instructor_id AS InstructorId,
                tr.message AS Message,
                tr.created_at AS ReplyCreatedAt
            FROM tickets t
            LEFT JOIN ticket_replies tr ON t.id = tr.ticket_id
            WHERE t.student_id = @StudentId
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
                    ticketDictionary.Add(existingTicket.Id, existingTicket);
                }

                if (reply is not null)
                {
                    existingTicket.Replies.Add(reply);
                }

                return existingTicket;
            },
            new { request.StudentId },
            splitOn: "ReplyId"
        );

        return ticketDictionary.Values.ToList();
    }
}
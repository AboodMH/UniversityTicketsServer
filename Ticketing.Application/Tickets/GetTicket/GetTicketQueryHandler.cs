using Dapper;
using Ticketing.Application.Abstractions.Data;
using Ticketing.Application.Abstractions.Messaging;
using Ticketing.Domain.Abstractions;
using Ticketing.Domain.Tickets;

namespace Ticketing.Application.Tickets.GetTicket;

internal sealed class GetTicketQueryHandler : IQueryHandler<GetTicketQuery, TicketResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetTicketQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<TicketResponse>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        using var connection = _sqlConnectionFactory.CreateConnection();

        const string sql = """
            SELECT
                id AS Id,
                student_id AS StudentId,
                title AS Title,
                description AS Description,
                status AS Status,
                created_at AS CreatedAt
            FROM tickets
            WHERE id = @TicketId
            """;

        var ticket = await connection.QueryFirstOrDefaultAsync<TicketResponse>(
            sql,
            new
            {
                request.TicketId
            });

        if (ticket is null)
        {
            return Result.Failure<TicketResponse>(TicketError.NotFound);
        }

        return ticket;
    }
}
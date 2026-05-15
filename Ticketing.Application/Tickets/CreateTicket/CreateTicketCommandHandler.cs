using Ticketing.Application.Abstractions.Clock;
using Ticketing.Application.Abstractions.Messaging;
using Ticketing.Domain.Abstractions;
using Ticketing.Domain.Tickets;
using Ticketing.Domain.Users;

namespace Ticketing.Application.Tickets.CreateTicket;

internal sealed class CreateTicketCommandHandler : ICommandHandler<CreateTicketCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateTicketCommandHandler(
        IUserRepository userRepository,
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _ticketRepository = ticketRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<Guid>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var student = await _userRepository.GetByIdAsync(request.StudentId, cancellationToken);

        if (student is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        Result<Ticket> ticketResult = Ticket.Create(
            student.Id,
            new Title(request.Title),
            new Description(request.Description),
            _dateTimeProvider.UtcNow);

        if (ticketResult.IsFailure)
        {
            return Result.Failure<Guid>(ticketResult.Error);
        }

        _ticketRepository.Add(ticketResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticketResult.Value.Id;
    }
}
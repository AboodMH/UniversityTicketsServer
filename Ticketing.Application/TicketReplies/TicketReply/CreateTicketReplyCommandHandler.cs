using Ticketing.Application.Abstractions.Clock;
using Ticketing.Application.Abstractions.Messaging;
using Ticketing.Domain.Abstractions;
using Ticketing.Domain.TicketReplies;
using Ticketing.Domain.Tickets;
using Ticketing.Domain.Users;

namespace Ticketing.Application.TicketReplies.TicketReply;

internal sealed class CreateTicketReplyCommandHandler : ICommandHandler<CreateTicketReplyCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly ITicketRepository _ticketRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTimeProvider;

    public CreateTicketReplyCommandHandler(
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

    public async Task<Result<Guid>> Handle(CreateTicketReplyCommand request, CancellationToken cancellationToken)
    {
        var instructor = await _userRepository.GetByIdAsync(request.InstructorId, cancellationToken);

        if (instructor is null)
        {
            return Result.Failure<Guid>(UserErrors.NotFound);
        }

        Ticket? ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);

        if (ticket is null)
        {
            return Result.Failure<Guid>(TicketError.NotFound);
        }

        Result replyResult = ticket.AddReply(
            instructor.Id,
            new Message(request.Message),
            _dateTimeProvider.UtcNow);

        if (replyResult.IsFailure)
        {
            return Result.Failure<Guid>(replyResult.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ticket.Id;
    }
}
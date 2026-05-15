using Ticketing.Domain.Abstractions;
using MediatR;

namespace Ticketing.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
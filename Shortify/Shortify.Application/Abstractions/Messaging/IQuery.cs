using MediatR;
using Shortify.Domain.Abstractions;

namespace Shortify.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

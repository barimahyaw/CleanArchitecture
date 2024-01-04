using Domain.Shared;
using MediatR;

namespace Application.Abstractions.Messaging;

public interface IQuery<IResponse> : IRequest<Result<IResponse>>
{
}

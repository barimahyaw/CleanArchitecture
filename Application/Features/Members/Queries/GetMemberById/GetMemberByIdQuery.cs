using Application.Abstractions.Messaging;

namespace Application.Features.Members.Queries.GetMemberById;

internal sealed record GetMemberByIdQuery(Guid MemberId) : IQuery<MemberResponse>;
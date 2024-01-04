using MediatR;

namespace Domain.Primitives;

// domain event are records of something that happened in the past within the domain
public interface IDomainEvent : INotification
{
}

// MemberCreatedDomainEvent
// GatheringsCreatedDomainEvent
// InvitationAcceptedDomainEvent
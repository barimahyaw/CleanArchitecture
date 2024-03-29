﻿using Domain.Primitives;

namespace Domain.DomainEvents;

public sealed record InvitationAcceptedDomainEvent(Guid InvitationId, Guid GatheringId) 
    : IDomainEvent
{
}


// records are immutable by default
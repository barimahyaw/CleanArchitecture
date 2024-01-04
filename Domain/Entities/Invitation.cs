using Domain.Enums;
using Domain.Primitives;

namespace Domain.Entities;

public class Invitation : Entity
{
    public Guid GatheringId { get; init; }
    public Guid MemberId { get; init; }
    public InvitationStatus Status { get; init; } = InvitationStatus.Pending;

    internal Attendee Accept()
    {
        throw new NotImplementedException();
    }

    internal void Expire()
    {
        throw new NotImplementedException();
    }
}

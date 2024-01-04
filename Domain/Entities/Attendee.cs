using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public class Attendee : Entity
{
    public Guid GatheringId { get; private set; }
    public Guid MemberId { get; private set; }
    public DateTime AcceptedAtUtc { get; private set; }        

    public Attendee(Guid id, Guid gatheringId, Guid memberId, DateTime acceptedAtUtc)
        : base(id)
    {
        GatheringId = gatheringId;
        MemberId = memberId;
        AcceptedAtUtc = acceptedAtUtc;
    }

    internal static ResultT<Attendee> Create(Attendee invitation)
    {
        var attendee = new Attendee(
            Guid.NewGuid(),
            invitation.GatheringId,
            invitation.MemberId,
            DateTime.UtcNow);

        return ResultT<Attendee>.Success(attendee);
    }
}

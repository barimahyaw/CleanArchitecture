using Domain.DomainEvents;
using Domain.Enums;
using Domain.Primitives;
using Domain.Shared;

namespace Domain.Entities;

public sealed class Gathering : AggregateRoot
{
    private readonly List<Invitation> _invitations = [];
    private readonly List<Attendee> _attendees = [];

    public Gathering(
        Guid id,
        Member creator,
        GatheringType type,
        DateTime scheduleAtUtc,
        string name,
        string? location) : base(id)
    {
        Creator = creator;
        Type = type;
        ScheduleAtUtc = scheduleAtUtc;
        Name = name;
        Location = location;
    }
    public Member Creator { get; }
    public GatheringType Type { get; }
    public DateTime ScheduleAtUtc { get; }
    public string Name { get; }
    public string? Location { get; }

    public IReadOnlyList<Invitation> Invitations  => _invitations;

    public int NumberOfAttendees { get; private set; }
    public int MaximumNumberOfAttendees { get; private set; } = 10;
    public DateTime InvitationsExpireAtUtc { get; private set; } = DateTime.UtcNow.AddDays(1);

    public Result<Attendee> AcceptInvitation(Invitation invitation)
    {
        var reachedMaximumNumberOfAttendees =
            Type == GatheringType.WithFixedNumberOfAttendees &&
            NumberOfAttendees == MaximumNumberOfAttendees;

        var reachedInvitationExpiration =
            Type == GatheringType.WithExpirationForInvitations &&
            InvitationsExpireAtUtc < DateTime.UtcNow;

        var expired = reachedMaximumNumberOfAttendees || reachedInvitationExpiration;

        if (expired)
        {
            invitation.Expire();
            return Result<Attendee>.Failure(DomainErrors.Gathering.Expired);
            //return Result<Attendee>.Failure("Invitation is expired.");
        }

        Attendee attendee = invitation.Accept();

        RaiseDomainEvent(new InvitationAcceptedDomainEvent(invitation.Id, invitation.GatheringId));

        var attendeeResult = Attendee.Create(attendee);

        if (!attendeeResult.IsSuccess || attendeeResult.Value is null)
            return Result<Attendee>.Failure(attendeeResult.ErrorMessage ?? "");

        if (invitation.Status != InvitationStatus.Pending)
        {
            return Result<Attendee>.Failure("Invitation is not pending.");
        }

        _attendees.Add(attendeeResult.Value);
        NumberOfAttendees++;

        return Result<Attendee>.Success(attendeeResult.Value);
    }
}
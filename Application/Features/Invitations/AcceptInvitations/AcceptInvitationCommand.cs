using Application.Abstractions.Messaging;

namespace Application.Features.Invitations.AcceptInvitations;

public sealed record AcceptInvitationCommand(Guid GatheringId, Guid InvitationId) : ICommand;

using Application.Abstractions.External;
using Domain.DomainEvents;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Invitations.Events;

internal sealed class InvitationAcceptedDomainEventHandler(IEmailService emailService, IGatheringRepository gatheringRepository)
        : INotificationHandler<InvitationAcceptedDomainEvent>
{
    private readonly IEmailService _emailService = emailService;
    private readonly IGatheringRepository _gatheringRepository = gatheringRepository;

    public async Task Handle(InvitationAcceptedDomainEvent notification, CancellationToken cancellationToken)
    {
        var gathering = await _gatheringRepository.GetByIdWithCreatorAsync(notification.GatheringId, cancellationToken);
        if (gathering is null) return;
        
        await _emailService.SendInvitationEmailAsync(gathering, cancellationToken);
    }
}

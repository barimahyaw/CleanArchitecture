using Application.Abstractions.External;
using Domain.Abstractions;
using Domain.DomainEvents;
using Domain.Entities;
using MediatR;

namespace Application.Features.Members.Events;

internal sealed class MemberRegisteredDomainEventHandler(
    IMemberRepository memberRepository, 
    IEmailService emailService) 
    : INotificationHandler<MemberRegisteredDomainEvent>
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IEmailService _emailService = emailService;

    public async Task Handle(
        MemberRegisteredDomainEvent notification, 
        CancellationToken cancellationToken)
    {
        Member? member = await _memberRepository.GetByIdAsync(
            notification.MemberId, 
            cancellationToken);

        if (member == null)
        {
            return;
        }

        await _emailService.SendWelcomeEmailAsync(member, cancellationToken);
    }
}

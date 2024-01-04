using Domain.Entities;

namespace Application.Abstractions.External;

internal interface IEmailService
{
    Task SendInvitationEmailAsync(Gathering gathering, CancellationToken cancellationToken = default);
    Task SendWelcomeEmailAsync(Member member, CancellationToken cancellationToken = default);
}

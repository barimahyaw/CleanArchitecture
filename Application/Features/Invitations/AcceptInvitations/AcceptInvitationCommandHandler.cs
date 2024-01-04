using Application.Abstractions.Messaging;
using Domain.Repositories;
using Domain.Shared;

namespace Application.Features.Invitations.AcceptInvitations;

internal sealed class AcceptInvitationCommandHandler : ICommandHandler<AcceptInvitationCommand>
{
    //private readonly IInvitationRepository _invitationRepository;
    private readonly IGatheringRepository _gatheringRepository;
    private readonly IAttendeeRepository _attendeeRepository;

    public AcceptInvitationCommandHandler(/*IInvitationRepository invitationRepository, */IGatheringRepository gatheringRepository, IAttendeeRepository attendeeRepository)
    {
        //_invitationRepository = invitationRepository;
        _gatheringRepository = gatheringRepository;
        _attendeeRepository = attendeeRepository;
    }

    public async Task<Result> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
    {
        var gathering = await _gatheringRepository
            .GetByIdWithCreatorAsync(request.GatheringId, cancellationToken);
        
        if (gathering is null)
        {
            return Result.Failure("Gathering does not exist." );
        }

        var invitation = gathering.Invitations
            .FirstOrDefault(i => i.Id == request.InvitationId);
        
        if (invitation is null)
        {
            return Result.Failure("Invitation does not exist.");
        }        

        var attendeeResult = gathering.AcceptInvitation(invitation);

        if (attendeeResult.IsSuccess && attendeeResult.Value is not null)
        {
            _attendeeRepository.Add(attendeeResult.Value);
        }

        return attendeeResult.IsSuccess
            ? Result.Success()
            : Result.Failure(attendeeResult.ErrorMessage ?? "");
    }
}

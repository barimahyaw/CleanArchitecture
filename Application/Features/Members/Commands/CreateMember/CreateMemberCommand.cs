using Application.Abstractions.Messaging;

namespace Application.Features.Members.Commands.CreateMember;

internal sealed record CreateMemberCommand(
    string Email,
    string FirstName,
    string LastName) : ICommand;
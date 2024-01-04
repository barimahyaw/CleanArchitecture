using Application.Abstractions.Messaging;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Shared;

namespace Application.Features.Members.Commands.CreateMember;

internal sealed class CreateMemberCommandHandler(
    IMemberRepository memberRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateMemberCommand>
{
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var isEmailUnique = await _memberRepository.IsEmailUniqueAsync(request.Email, cancellationToken);

        var memberResult = Member.Create(
            Guid.NewGuid(),
            request.Email,
            request.FirstName,
            request.LastName,
            isEmailUnique);

        if (!memberResult.IsSuccess || memberResult.Value is null) return Result.Failure(memberResult.ErrorMessage ?? "");
        
        _memberRepository.Add(memberResult.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

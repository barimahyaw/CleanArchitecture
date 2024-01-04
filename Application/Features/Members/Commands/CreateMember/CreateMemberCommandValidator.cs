using Domain.ValueObjects;
using FluentValidation;

namespace Application.Features.Members.Commands.CreateMember;

internal class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(FirstName.MaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(LastName.MaxLength);
    }
}

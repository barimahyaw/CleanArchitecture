using Domain.DomainEvents;
using Domain.Primitives;
using Domain.Shared;
using Domain.ValueObjects;

namespace Domain.Entities;

public sealed class Member : AggregateRoot
{
    public Member(Guid id, Email email, FirstName firstName, LastName lastName)
        : base(id)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public Email Email { get; }
    public FirstName FirstName { get; }
    public LastName LastName { get; }

    public static ResultT<Member> Create(
        Guid id, 
        string email, 
        string firstName, 
        string lastName,
        bool isEmailUnique)
    {
        if (isEmailUnique)
        {
            return ResultT<Member>.Failure("Email is not unique.");  
        }

        var emailResult = Email.Create(email);
        if (!emailResult.IsSuccess || emailResult.Value is null) 
            return ResultT<Member>.Failure(emailResult.ErrorMessage ?? "Email value is invalid.");

        var firstNameResult = FirstName.Create(firstName);
        if (!firstNameResult.IsSuccess || firstNameResult.Value is null) 
            return ResultT<Member>.Failure(firstNameResult.ErrorMessage ?? "First name value is invalid.");

        var lastNameResult = LastName.Create(lastName);
        if (!lastNameResult.IsSuccess || lastNameResult.Value is null) 
            return ResultT<Member>.Failure(lastNameResult.ErrorMessage ?? "Last name value is invalid.");

        var member = new Member(id, emailResult.Value, firstNameResult.Value, lastNameResult.Value);
        
        member.RaiseDomainEvent(new MemberRegisteredDomainEvent(Guid.NewGuid(), member.Id));

        return ResultT<Member>.Success(member);
    }
}

using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class FirstName(string value) : ValueObject
{
    public string Value { get; private set; } = value;

    //private FirstName(string value) => Value = value;

    public const int MaxLength = 50;
    public static Result<FirstName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName)) 
        {
            return Result<FirstName>.Failure("The first name cannot be empty.");
        }

        if (firstName.Length > MaxLength) 
        {
            return Result<FirstName>.Failure("The first name cannot be longer than 50 characters.");
        }

        return Result<FirstName>.Success(new FirstName(firstName));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}

using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public sealed class LastName : ValueObject
{
    public override IEnumerable<object> GetAtomicValues()
    {
       yield return Value;
    }

    public static Result<LastName> Create(string lastName)
    {

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result<LastName>.Failure("The last name cannot be empty.");
        }

        if (lastName.Length > MaxLength)
        {
            return Result<LastName>.Failure("The last name cannot be longer than 50 characters.");
        }

        return Result<LastName>.Success(new LastName(lastName));
    }

    public const int MaxLength = 50;

    public string Value { get; private set; } 

    private LastName(string value) => Value = value;
}

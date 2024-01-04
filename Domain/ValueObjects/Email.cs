using Domain.Primitives;
using Domain.Shared;

namespace Domain.ValueObjects;

public class Email(string value) : ValueObject
{
    public string Address { get; private set; } = value;

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result<Email>.Failure("The email address cannot be empty.");
        }

        // validate email address format
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            if (addr.Address != email)
            {
                return Result<Email>.Failure("The email address is not valid.");
            }
        }
        catch
        {
            return Result<Email>.Failure("The email address is not valid.");
        }

        var isValidEmailAddress = email.Split('@').Length != 2;
        if (isValidEmailAddress)
        {
            return Result<Email>.Failure("The email address is not valid.");
        }

        return Result<Email>.Success(new Email(email));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Address;
    }
}
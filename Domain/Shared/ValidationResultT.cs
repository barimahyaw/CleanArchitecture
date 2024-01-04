namespace Domain.Shared;

public class ValidationResultT<T> : Result<T>, IValidationResult
{
    private ValidationResultT(Error[] errors)
        : base(default, default, false, IValidationResult.ValidationError) =>
        Errors = errors;

    public Error[] Errors { get; }

    public static ValidationResultT<T> WithErrors(Error[] errors) =>
        new(errors);
}

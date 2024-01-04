namespace Domain.Shared;

public record ResultT<T>
{
    public bool IsSuccess { get; private init; }
    public T? Value { get; private init; }
    public string? ErrorMessage { get; private init; }

    private ResultT() { }

    public static ResultT<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static ResultT<T> Failure(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}

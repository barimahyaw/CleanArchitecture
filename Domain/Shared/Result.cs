using Domain.Entities;

namespace Domain.Shared;

public class Result
{   
    public bool IsSuccess { get; private init; }
    public string? ErrorMessage { get; private init; }

    private Result() { }

    protected Result(string? errorCode, bool isSuccess, Error error)
    {
        _ = errorCode;
        IsSuccess = isSuccess;
        ErrorMessage = error.Message;
    }


    public static Result Success() => new() { IsSuccess = true };
    public static Result Failure(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}

public class Result<T>
{
    public bool IsSuccess { get; private init; }
    public T? Value { get; private init; }
    public string? ErrorMessage { get; private init; }

    private Result() { }

    protected Result(string? errorCode, T? value, bool isSuccess, Error error)
    {
        _ = errorCode;
        Value = value;
        IsSuccess = isSuccess;
        ErrorMessage = error.Message;
    }

    public static Result<T> Success(T value) => new() { IsSuccess = true, Value = value };
    public static Result<T> Failure(string errorMessage) => new()
    {
        IsSuccess = false,
        ErrorMessage = errorMessage
    };
}

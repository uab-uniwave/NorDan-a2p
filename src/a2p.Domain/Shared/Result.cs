
namespace a2p.Domain.Shared;

public class Result
{
    public bool IsSuccess { get; }
    public string? Message { get; }
    public string? ErrorCode { get; }

    protected Result(bool isSuccess, string? message = null, string? errorCode = null)
    {
        IsSuccess = isSuccess;
        Message = message;
        ErrorCode = errorCode;
    }

    public static Result Success(string? message = null)
        => new(true, message);

    public static Result Failure(string message, string? errorCode = null)
        => new(false, message, errorCode);
}

public class Result<T> : Result
{
    public T? Value { get; }

    // 🔹 must be protected so derived classes (PagedResult, ValidationResult) can call it
    protected Result(bool isSuccess, T? value, string? message = null, string? errorCode = null)
        : base(isSuccess, message, errorCode)
    {
        Value = value;
    }

    public static Result<T> Success(T value, string? message = null)
        => new(true, value, message);

    public static new Result<T> Failure(string message, string? errorCode = null)
        => new(false, default, message, errorCode);
}
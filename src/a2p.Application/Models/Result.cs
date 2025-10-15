public class Result
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public string ErrorMessage { get; private set; } = string.Empty;
    public string ErrorCode { get; private set; } = string.Empty;
    public Exception? Exception { get; private set; }

    protected Result(bool isSuccess, string errorMessage = "", string errorCode = "", Exception? exception = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
        Exception = exception;
    }

    public static Result Success() => new(true);
    public static Result Failure(string errorMessage, string errorCode = "", Exception? exception = null)
        => new(false, errorMessage, errorCode, exception);

    public static Result<T> Success<T>(T value) => new(value, true);
    public static Result<T> Failure<T>(string errorMessage, string errorCode = "", Exception? exception = null)
        => new(default!, false, errorMessage, errorCode, exception);
}

public class Result<T> : Result
{
    public T? Value { get; private set; }

    internal Result(T value, bool isSuccess, string errorMessage = "", string errorCode = "", Exception? exception = null)
        : base(isSuccess, errorMessage, errorCode, exception)
    {
        Value = value;
    }

    public static implicit operator Result<T>(T value) => Success(value);
}

// ============================================
// DETAILED OPERATION RESULT
// ============================================

public class OperationResult<T>
{
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public string ErrorCode { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
    public Exception? Exception { get; set; }
    public int RowsAffected { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public static OperationResult<T> Success(T data, string message = "Operation completed successfully", int rowsAffected = 0)
    {
        return new OperationResult<T>
        {
            IsSuccess = true,
            Data = data,
            Message = message,
            RowsAffected = rowsAffected
        };
    }

    public static OperationResult<T> Failure(string errorMessage, string errorCode = "ERROR", Exception? exception = null)
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Message = errorMessage,
            ErrorCode = errorCode,
            Exception = exception,
            Errors = new List<string> { errorMessage }
        };
    }

    public static OperationResult<T> Failure(List<string> errors, string errorCode = "ERROR")
    {
        return new OperationResult<T>
        {
            IsSuccess = false,
            Message = "Operation failed with multiple errors",
            ErrorCode = errorCode,
            Errors = errors
        };
    }
}

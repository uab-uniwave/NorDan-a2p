namespace a2p.Application.Models
{
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
}

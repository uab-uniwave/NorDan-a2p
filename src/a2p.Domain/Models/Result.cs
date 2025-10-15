namespace a2p.Domain.Models
{
    /// <summary>
    /// A generic result class to encapsulate success or failure outcomes.
    /// </summary>
    /// <typeparam name="T">The type of the value in case of success.</typeparam>   
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string? Error { get; }
        public T? Value { get; }

        private Result(T value)
        {
            IsSuccess = true;
            Value = value;
        }

        private Result(string error)
        {
            IsSuccess = false;
            Error = error;
        }

        public static Result<T> Success(T value) => new(value);
        public static Result<T> Failure(string error) => new(error);
    }
}
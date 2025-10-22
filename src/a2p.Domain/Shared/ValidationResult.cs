namespace Domain.Shared
{
    public class ValidationResult<T> : Result<T>
    {
        public IReadOnlyList<ValidationError> Errors { get; }

        private ValidationResult(
            bool isSuccess,
            T? value,
            string? message,
            IEnumerable<ValidationError>? errors = null)
            : base(isSuccess, value, message) // ✅ fixed signature
        {
            Errors = errors?.ToList() ?? new List<ValidationError>();
        }

        public static ValidationResult<T> Success(T value, string? message = null)
            => new(true, value, message);

        public static ValidationResult<T> Failure(IEnumerable<ValidationError> errors, string? message = null)
            => new(false, default, message ?? "Validation failed.", errors);
    }
}
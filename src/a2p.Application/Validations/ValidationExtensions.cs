using a2p.Domain.Shared;

namespace a2p.Application.Validations
{
    public static class ValidationExtensions
    {
        public static ValidationResult<T> ToValidationResult<T>(
            this FluentValidation.Results.ValidationResult fluentResult,
            T? value = default)
        {
            if (fluentResult.IsValid)
                return ValidationResult<T>.Success(value!);

            var errors = fluentResult.Errors
                .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))
                .ToList();

            return ValidationResult<T>.Failure(errors);
        }
    }
}
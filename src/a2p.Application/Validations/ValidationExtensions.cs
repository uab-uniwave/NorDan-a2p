using Domain.Shared;

namespace Application.Validations
{
    public static class ValidationExtensions
    {
        public static ValidationResult<T> ToValidationResult<T>(
        this FluentValidation.Results.ValidationResult fluentResult,
        T? value = default)
        {
            if (fluentResult.IsValid)
            {
                return ValidationResult<T>.Success(value!);
            }

            List<ValidationError> errors = fluentResult.Errors
            .Select(e => new ValidationError(e.PropertyName, e.ErrorMessage))
            .ToList();

            return ValidationResult<T>.Failure(errors);
        }
    }
}
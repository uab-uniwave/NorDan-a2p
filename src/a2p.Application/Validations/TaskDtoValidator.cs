using Application.DTOs;

using FluentValidation;

namespace Application.Validations
{
    public class TaskDtoValidator : AbstractValidator<TaskDto>
    {
        public TaskDtoValidator()
        {
            RuleFor(x => x.OrderNumber)
            .NotEmpty().WithMessage("OrderNumber number is required.")
            .MaximumLength(50).WithMessage("OrderNumber number too long.");

            RuleFor(x => x.OrderId)
            .NotEqual(Guid.Empty).WithMessage("OrderId must be provided.");

            RuleFor(x => x.ProjectNumber)
            .MaximumLength(50);

            RuleFor(x => x.SalesDocumentNumber)
            .GreaterThanOrEqualTo(-1);

            RuleFor(x => x.SalesDocumentVersion)
            .GreaterThanOrEqualTo(-1);

            RuleFor(x => x.PayloadJson)
            .NotNull();

            RuleFor(x => x.State)
            .GreaterThanOrEqualTo(0).WithMessage("State must be greater than or equal to 0.");
        }
    }
}
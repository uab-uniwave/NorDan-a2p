using a2p.Application.DTOs;

using FluentValidation;

namespace a2p.Application.Validations
{
    public class TaskValidator : AbstractValidator<TaskDto>
    {
        public TaskValidator()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty().WithMessage("Order number is required.")
                .MaximumLength(50).WithMessage("Order number too long.");

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
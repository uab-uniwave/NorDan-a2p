using a2p.Application.DTOs;

using FluentValidation;

namespace a2p.Application.Validations
{
    public class OrderValidator : AbstractValidator<OrderDto>
    {
        public OrderValidator()
        {
            // --- Required fields ---
            RuleFor(x => x.OrderNumber)
                .NotEmpty().WithMessage("Order number is required.")
                .MaximumLength(50).WithMessage("Order number too long.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Length(3).WithMessage("Currency must be a 3-letter ISO code.");

            // SalesDocument basic checks
            RuleFor(x => x.SalesDocument)
                .NotNull().WithMessage("SalesDocument must be provided.");

            When(x => x.SalesDocument != null, () =>
            {
                RuleFor(x => x.SalesDocument.Number)
                    .GreaterThan(0).WithMessage("SalesDocument.Number must be greater than zero.");
                RuleFor(x => x.SalesDocument.Version)
                    .GreaterThanOrEqualTo(0).WithMessage("SalesDocument.Version must be greater than or equal to zero.");
            });

            // --- Numeric sanity ---
            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Total price cannot be negative.");

            RuleFor(x => x.ExchangeRate)
                .GreaterThan(0).WithMessage("Exchange rate must be greater than zero.");

            // --- Dates ---
            RuleFor(x => x.ExchangeRateDate)
                .NotEqual(default(DateOnly)).WithMessage("Exchange rate date is required.");

            // --- Collections ---
            RuleFor(x => x.ItemsDto)
                .NotNull().WithMessage("Items list must be provided.")
                .Must(i => i.Count > 0).WithMessage("Item quantity must be greater than zero.");

            RuleForEach(x => x.ItemsDto).SetValidator(new ItemValidator());

            RuleFor(x => x.MaterialsDto)
                .NotNull().WithMessage("Materials list must be provided.")
                .Must(m => m.Count > 0).WithMessage("Materials quantity must be greater than zero.");

            RuleForEach(x => x.MaterialsDto).SetValidator(new MaterialValidator());

            // --- Conditional examples / enums ---
            RuleFor(x => x.SourceAppType)
                .NotEqual(Domain.Enums.SourceAppType.Unknown)
                .WithMessage("SourceAppType must be defined.");
        }
    }
}
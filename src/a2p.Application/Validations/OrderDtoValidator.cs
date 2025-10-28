using Application.DTOs;

using FluentValidation;

namespace Application.Validations
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        public OrderDtoValidator()
        {
            // --- Required fields ---
            RuleFor(x => x.OrderNumber)
                .NotEmpty().WithMessage("OrderNumber number is required.")
                .MaximumLength(50).WithMessage("OrderNumber number too long.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Length(3).WithMessage("Currency must be a 3-letter ISO code.");

            // SalesDocumentDto basic checks
            RuleFor(x => x.SalesDocument)
                .NotNull().WithMessage("SalesDocumentDto must be provided.");

            When(x => x.SalesDocument != null, () =>
            {
                RuleFor(x => x.SalesDocument.Number)
                    .GreaterThan(0).WithMessage("SalesDocumentDto.Number must be greater than zero.");
                RuleFor(x => x.SalesDocument.Version)
                    .GreaterThanOrEqualTo(0).WithMessage("SalesDocumentDto.Version must be greater than or equal to zero.");
            });

            RuleFor(x => x.ExchangeRate)
                .GreaterThan(0).WithMessage("Exchange rate must be greater than zero.");

            // --- Dates ---
            //RuleFor(x => x.ExchangeRateDate)
            //    .NotEqual(default(DateOnly)).WithMessage("Exchange rate date is required.");

            // --- Collections ---
            RuleFor(x => x.ItemsDto)
                .NotNull().WithMessage("Items list must be provided.")
                .Must(i => i.Count > 0).WithMessage("Item quantity must be greater than zero.");

            RuleForEach(x => x.ItemsDto).SetValidator(new ItemDtoValidator());

            RuleFor(x => x.MaterialsDto)
                .NotNull().WithMessage("Materials list must be provided.")
                .Must(m => m.Count > 0).WithMessage("Materials quantity must be greater than zero.");

            RuleForEach(x => x.MaterialsDto).SetValidator(new MaterialDtoValidator());

        }
    }
}
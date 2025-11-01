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

            // --- Optional fields with conditional validation ---
            // Currency is only validated if provided
            When(x => !string.IsNullOrEmpty(x.Currency), () =>
            {
                RuleFor(x => x.Currency)
                .Length(3).WithMessage("Currency must be a 3-letter ISO code.");
            });

            // SalesDocumentDto basic checks - only if Number > 0 (meaning it was explicitly set)
            When(x => x.SalesDocument?.Number > 0, () =>
            {
                RuleFor(x => x.SalesDocument.Number)
                .GreaterThan(0).WithMessage("SalesDocumentDto.Number must be greater than zero.");
                RuleFor(x => x.SalesDocument.Version)
                .GreaterThanOrEqualTo(0).WithMessage("SalesDocumentDto.Version must be greater than or equal to zero.");
            });

            // ExchangeRate is only validated if greater than 0
            When(x => x.ExchangeRate > 0, () =>
            {
                RuleFor(x => x.ExchangeRate)
                .GreaterThan(0).WithMessage("Exchange rate must be greater than zero.");
            });

            // --- Collections - validate only if provided with items ---
            When(x => x.ItemsDto != null && x.ItemsDto.Count > 0, () =>
            {
                RuleForEach(x => x.ItemsDto).SetValidator(new ItemDtoValidator());
            });

            When(x => x.MaterialsDto != null && x.MaterialsDto.Count > 0, () =>
            {
                RuleForEach(x => x.MaterialsDto).SetValidator(new MaterialDtoValidator());
            });
        }
    }
}
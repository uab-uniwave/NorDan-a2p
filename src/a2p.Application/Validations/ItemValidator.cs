using a2p.Application.DTOs;

using FluentValidation;

namespace a2p.Application.Validations
{
    public class ItemValidator : AbstractValidator<ItemDto>
    {
        public ItemValidator()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty().WithMessage("Order number is required.")
                .MaximumLength(50).WithMessage("Order number too long, max length 50.");

            RuleFor(x => x.ItemName)
                .NotEmpty().WithMessage("ItemName is required.")
                .MaximumLength(50).WithMessage("ItemName too long, max length 50.");

            RuleFor(x => x.Worksheet)
                .MaximumLength(255).WithMessage("Worksheet too long, max length 255.");

            RuleFor(x => x.Line)
                .GreaterThanOrEqualTo(0).WithMessage("Line must be greater than or equal to 0.");

            RuleFor(x => x.Column)
                .GreaterThanOrEqualTo(0).WithMessage("Column must be greater than or equal to 0.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

            RuleFor(x => x.Width).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Height).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Weight).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalWeight).GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaterialCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.LaborCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0);

            RuleFor(x => x.TotalMaterialCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalLaborCost).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalCost).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalPrice).GreaterThanOrEqualTo(0);

            RuleFor(x => x.SalesDocumentNumber)
                .GreaterThanOrEqualTo(0).WithMessage("SalesDocumentNumber must be greater than or equal to 0.");

            RuleFor(x => x.SalesDocumentVersion)
                .GreaterThanOrEqualTo(0).WithMessage("SalesDocumentVersion must be greater than or equal to 0.");

            RuleFor(x => x.WorksheetType)
                .NotEqual(Domain.Enums.WorksheetType.Unknown)
                .WithMessage("WorksheetType must be defined.");
        }
    }
}
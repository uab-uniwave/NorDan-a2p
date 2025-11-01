using Application.DTOs;

using FluentValidation;

namespace Application.Validations
{
    public class ItemDtoValidator : AbstractValidator<ItemDto>
    {
        public ItemDtoValidator()
        {
            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Item Id number is required.");

            RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("Order Id is required.");

            RuleFor(x => x.ItemName)
            .NotEmpty().WithMessage("ItemName is required.")
            .MaximumLength(50).WithMessage("Item Name too long, max length 50.");

            RuleFor(x => x.Worksheet)
            .MaximumLength(255).WithMessage("Worksheet name too long, max length 255.");

            RuleFor(x => x.Line)
            .GreaterThanOrEqualTo(0).WithMessage("Cell Line must be greater than or equal to 0.");

            RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

            RuleFor(x => x.Width).GreaterThan(0).WithMessage("Width must be greater than 0.");
            RuleFor(x => x.Height).GreaterThan(0).WithMessage("Height must be greater than 0.");

            RuleFor(x => x.Weight).GreaterThan(0).WithMessage("Weight must be greater than 0."); ;
            RuleFor(x => x.TotalWeight).GreaterThan(0).WithMessage("TotalWeight must be greater than 0."); ; ;

            RuleFor(x => x.MaterialCost).GreaterThanOrEqualTo(0).WithMessage("Material Cost must be greater than or eqaul to 0.");
            RuleFor(x => x.LaborCost).GreaterThanOrEqualTo(0).WithMessage("Labor Cost must be greater than or eqaul to 0.");
            RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).WithMessage("Cost must be greater than or eqaul to 0.");

            RuleFor(x => x.TotalMaterialCost).GreaterThanOrEqualTo(0).WithMessage("Total Material Cost must be greater than or eqaul to 0.");
            RuleFor(x => x.TotalLaborCost).GreaterThanOrEqualTo(0).WithMessage("Total Labor Cost must be greater than or eqaul to 0.");
            RuleFor(x => x.TotalCost).GreaterThanOrEqualTo(0).WithMessage("Total Cost must be greater than or eqaul to 0.");

            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be greater than 0.");
            RuleFor(x => x.TotalPrice).GreaterThanOrEqualTo(0).WithMessage("Total Price must be greater than or eqaul to 0.");

            RuleFor(x => x.WorksheetType)
            .NotEqual(Domain.Enums.WorksheetType.Unknown)
            .WithMessage("WorksheetType must be defined.");
        }
    }
}
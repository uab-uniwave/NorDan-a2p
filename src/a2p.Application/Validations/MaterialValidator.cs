using a2p.Application.DTOs;
using a2p.Domain.Enums;

using FluentValidation;

namespace a2p.Application.Validations
{
    public class MaterialValidator : AbstractValidator<MaterialDto>
    {
        public MaterialValidator()
        {
            // --- Required / basic fields ---
            RuleFor(x => x.OrderNumber)
                .NotEmpty().WithMessage("Order number is required.")
                .MaximumLength(50).WithMessage("Order number too long, max length 50.");

            RuleFor(x => x.ReferenceBase)
                .NotEmpty().WithMessage("ReferenceBase is required.")
                .MaximumLength(25).WithMessage("ReferenceBase too long, max length 25.");

            RuleFor(x => x.Reference)
                .NotEmpty().WithMessage("Reference is required.")
                .MaximumLength(25).WithMessage("Reference too long, max length 25.");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Color is required.")
                .MaximumLength(50).WithMessage("Color too long, max length 50.");

            // --- String length constraints for optional fields ---
            RuleFor(x => x.Worksheet)
                .MaximumLength(255).WithMessage("Worksheet too long, max length 255.");

            RuleFor(x => x.Description)
                .MaximumLength(255).WithMessage("Description too long, max length 255.");

            RuleFor(x => x.ColorDescription)
                .MaximumLength(120).WithMessage("ColorDescription too long, max length 120.");

            RuleFor(x => x.Pallet)
                .MaximumLength(255).WithMessage("Pallet too long, max length 255.");

            RuleFor(x => x.CustomField1).MaximumLength(255);
            RuleFor(x => x.CustomField2).MaximumLength(255);
            RuleFor(x => x.CustomField3).MaximumLength(255);
            RuleFor(x => x.CustomField4).MaximumLength(255);
            RuleFor(x => x.CustomField5).MaximumLength(255);

            RuleFor(x => x.SourceReference).MaximumLength(255);
            RuleFor(x => x.SourceDescription).MaximumLength(255);
            RuleFor(x => x.SourceColor).MaximumLength(255);
            RuleFor(x => x.SourceColorDescription).MaximumLength(255);

            // --- Numeric sanity ---
            RuleFor(x => x.Line)
                .GreaterThanOrEqualTo(0).WithMessage("Line must be greater than or equal to 0.");

            RuleFor(x => x.Column)
                .GreaterThanOrEqualTo(0).WithMessage("Column must be greater than or equal to 0.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity cannot be negative.");

            RuleFor(x => x.RequiredQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("RequiredQuantity cannot be negative.");

            RuleFor(x => x.PackageQuantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalQuantity).GreaterThanOrEqualTo(0);
            RuleFor(x => x.LeftOverQuantity).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.RequiredPrice).GreaterThanOrEqualTo(0);
            RuleFor(x => x.LeftOverPrice).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Width).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Height).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Weight).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalWeight).GreaterThanOrEqualTo(0);
            RuleFor(x => x.RequiredWeight).GreaterThanOrEqualTo(0);
            RuleFor(x => x.LeftOverWeight).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Area).GreaterThanOrEqualTo(0);
            RuleFor(x => x.TotalArea).GreaterThanOrEqualTo(0);
            RuleFor(x => x.RequiredArea).GreaterThanOrEqualTo(0);
            RuleFor(x => x.LeftOverArea).GreaterThanOrEqualTo(0);

            RuleFor(x => x.Waste).GreaterThanOrEqualTo(0);
            RuleFor(x => x.SquareMeterPrice).GreaterThanOrEqualTo(0);

            // Commodity code if present must be non-negative
            When(x => x.CommodityCode.HasValue, () =>
            {
                RuleFor(x => x.CommodityCode.Value).GreaterThanOrEqualTo(0);
            });

            // --- Enum validations ---
            RuleFor(x => x.MaterialType)
                .NotEqual(MaterialType.Unknown)
                .WithMessage("MaterialType must be defined.");

            RuleFor(x => x.WorksheetType)
                .NotEqual(Domain.Enums.WorksheetType.Unknown)
                .WithMessage("WorksheetType must be defined.");
        }
    }
}

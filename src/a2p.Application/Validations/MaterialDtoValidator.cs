using Application.DTOs;

using Domain.Enums;

using FluentValidation;

namespace Application.Validations
{
    public class MaterialDtoValidator : AbstractValidator<MaterialDto>
    {
        public MaterialDtoValidator()
        {
            // --- Required / basic fields ---
            RuleFor(x => x.OrderId)
                 .NotEmpty().WithMessage("OrderId number is required.");
                
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

            RuleFor(x => x.CustomField1).MaximumLength(255).WithMessage("CustomField1 too long, max length 255.");
            RuleFor(x => x.CustomField2).MaximumLength(255).WithMessage("CustomField2 too long, max length 255.");
            RuleFor(x => x.CustomField3).MaximumLength(255).WithMessage("CustomField3 too long, max length 255.");
            RuleFor(x => x.CustomField4).MaximumLength(255).WithMessage("CustomField4 too long, max length 255.");
            RuleFor(x => x.CustomField5).MaximumLength(255).WithMessage("CustomField5 too long, max length 255.");

            RuleFor(x => x.SourceReference).MaximumLength(255).WithMessage("SourceReference too long, max length 255."); ;
            RuleFor(x => x.SourceDescription).MaximumLength(255).WithMessage("SourceDescription too long, max length 255."); ;
            RuleFor(x => x.SourceColor).MaximumLength(255).WithMessage("SourceColor too long, max length 255."); ;
            RuleFor(x => x.SourceColorDescription).MaximumLength(255).WithMessage("SourceColorDescription too long, max length 255."); ;

            // --- Numeric sanity ---
            RuleFor(x => x.Line)
                .GreaterThanOrEqualTo(0).WithMessage("Line must be greater than or equal to 0.");

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0");

            RuleFor(x => x.RequiredQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("RequiredQuantity must be greater than or equal to 0");

            // Commodity code if present must be non-negative
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

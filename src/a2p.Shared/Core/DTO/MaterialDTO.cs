namespace a2p.Shared.Core.DTO
{
    public class MaterialDTO : BaseDTO
    {



        public string Color { get; set; } = string.Empty;
        public string CustomField1 { get; set; } = string.Empty; //Custom field (used for color just SapaV1)
        public string CustomField2 { get; set; } = string.Empty; //Custom field (used for color just SapaV1)
        public string CustomField3 { get; set; } = string.Empty; //Custom field (used for color just SapaV1)
        public string ColorDescription { get; set; } = string.Empty;//

        public double PackageUnit { get; set; } = 0; // For Profiles it's length of Bar
        public double QuantityOrdered { get; set; } = 0; //
        public double QuantityRequired { get; set; } = 0;
        public double Waste { get; set; } = 0;
        public double Weight { get; set; } = 0;


    }
}
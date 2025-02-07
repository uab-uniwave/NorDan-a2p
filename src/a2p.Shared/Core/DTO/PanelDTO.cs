namespace a2p.Shared.Core.DTO
{
    public class PanelDTO : BaseDTO
    {

        //public string Worksheet { get; set; } // WorkSheet name

        public int SortOrder { get; set; } = 0; //SortOrder or SortOrder

        public double Width { get; set; } = 0;
        public double Height { get; set; } = 0;

        public double TotalArea { get; set; } = 0; //same as TotalAreaPerKind
        public double Ordered { get; set; } = 0; //same as TotalAreaPerKind
        public double AreaUsed { get; set; } = 0; //same as TotalArea
        public double Waste { get; set; } = 0; //same as Waste

        public string ColorDescription { get; set; } = string.Empty;//same as SurfaceDescription 
        public string CustomField1 { get; set; } = string.Empty;
        public string CustomField2 { get; set; } = string.Empty;
        public string CustomField3 { get; set; } = string.Empty;
        public decimal SquareMeterPrice { get; set; } = 0;


        public int Count { get; set; } = 0;

    }

}
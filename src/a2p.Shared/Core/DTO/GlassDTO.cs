namespace a2p.Shared.Core.DTO
{
 public class GlassDTO : BaseDTO
 {


  public int SortOrder { get; set; } = 0;
  public double Width { get; set; } = 0;
  public double Height { get; set; } = 0;
  public double Weight { get; set; } = 0;
  public double TotalWeight { get; set; } = 0;
  public double TotalArea { get; set; } = 0; //per piece * quantity
  public double AreaUsed { get; set; } = 0; //normally used in panels TODO: check if needed
  public double AreaOrdered { get; set; } = 0; //normally used in panels TODO: check if needed
  public decimal SquareMeterPrice { get; set; } = 0;
  public string Pallet { get; set; } = string.Empty;

 }
}

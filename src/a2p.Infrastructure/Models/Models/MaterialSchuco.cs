namespace a2p.Shared.Application.Models
{
 public class MaterialSchuco
 {
  public string File { get; set; } = string.Empty;
  public string Worksheet { get; set; } = string.Empty;
  public string Order { get; set; } = string.Empty;
  public string Article { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string Price { get; set; } = "0";
  public string Quantity { get; set; } = "0";
  public string Delivery { get; set; } = "0";
  public string Dimensions { get; set; } = "0";
  public string Weight { get; set; } = "0";
  public string TotalPrice { get; set; } = "0";
 }
}

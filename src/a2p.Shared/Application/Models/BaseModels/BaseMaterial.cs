namespace a2p.Shared.Application.Models.BaseModels
{
 // Base class for all materials
 public abstract class BaseMaterial
 {
  public string File { get; set; } = string.Empty;
  public string Worksheet { get; set; } = string.Empty;
  public string Order { get; set; } = string.Empty;
  public string Article { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;
  public string Quantity { get; set; } = "0";
  public string Weight { get; set; } = "0";
  public string Price { get; set; } = "0";
  public string Package { get; set; } = "0";
  public string Color { get; set; } = string.Empty;
 }

}


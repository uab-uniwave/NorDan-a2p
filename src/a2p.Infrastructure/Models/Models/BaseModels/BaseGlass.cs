namespace a2p.Shared.Application.Models.BaseModels
{
 public abstract class BaseGlass : BaseMaterial
 {
  public string Width { get; set; } = "0";
  public string Height { get; set; } = "0";
  public string Area { get; set; } = "0";
  public string TotalArea { get; set; } = "0";
  public string SquareMeterPrice { get; set; } = "0";
 }
}

namespace a2p.Shared.Core.Entities.BaseEntities
{
 public abstract class BasePanel : BaseMaterial
 {
  public string AreaOrdered { get; set; } = "0";
  public string reaRequired { get; set; } = "0";
  public string Waste { get; set; } = "0";
 }
}

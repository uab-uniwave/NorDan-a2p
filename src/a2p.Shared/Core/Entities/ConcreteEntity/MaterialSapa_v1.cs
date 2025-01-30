using a2p.Shared.Core.Entities.BaseEntities;

namespace a2p.Shared.Core.Entities.ConcreteEntity
{
 public class MaterialSapa_v1 : BaseMaterial
 {
  public string CustomField1 { get; set; } = string.Empty;
  public string CustomField2 { get; set; } = string.Empty;
  public string CustomField3 { get; set; } = string.Empty;

  public string QuantityOrdered { get; set; } = "0";
  public string QuantityRequired { get; set; } = "0";
  public string Waste { get; set; } = "0";
  public string Currency { get; set; } = "0";
 }
}

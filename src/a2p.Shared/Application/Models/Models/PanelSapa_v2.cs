using a2p.Shared.Core.Entities.BaseEntities;

namespace a2p.Shared.Core.Entities.ConcreteEntity
{
 public class PanelSapa_v2 : BasePanel
 {
  public string ArticleType { get; set; } = string.Empty;
  public string ColorDescription { get; set; } = string.Empty;
  public string SquareMeterPrice { get; set; } = string.Empty;
 }
}
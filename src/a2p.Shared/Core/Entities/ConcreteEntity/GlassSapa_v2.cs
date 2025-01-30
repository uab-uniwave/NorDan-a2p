using a2p.Shared.Core.Entities.BaseEntities;

namespace a2p.Shared.Core.Entities.ConcreteEntity
{
 public class GlassSapa_v2 : BaseGlass
 {
  public string ArticleType { get; set; } = string.Empty;
  public string Palette { get; set; } = string.Empty;
  public string TotalWeight { get; set; } = string.Empty;
 }
}
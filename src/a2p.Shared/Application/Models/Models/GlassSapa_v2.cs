using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Application.Models.Models
{
 public class GlassSapa_v2 : BaseGlass
 {
  public string ArticleType { get; set; } = string.Empty;
  public string Palette { get; set; } = string.Empty;
  public string TotalWeight { get; set; } = string.Empty;
 }
}
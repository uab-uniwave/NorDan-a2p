using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Application.Models
{
 public class GlassTechDesign : BaseGlass
 {
  public string ArticleType { get; set; } = string.Empty;
  public string Palette { get; set; } = string.Empty;
  public string TotalWeight { get; set; } = string.Empty;
 }
}
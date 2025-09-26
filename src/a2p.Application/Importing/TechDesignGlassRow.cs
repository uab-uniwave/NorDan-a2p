using a2p.Application.Importing.BaseModels;

namespace a2p.Application.Importing
{
 public class TechDesignGlassRow : BaseGlass
 {
  public string ArticleType { get; set; } = string.Empty;
  public string Palette { get; set; } = string.Empty;
  public string TotalWeight { get; set; } = string.Empty;
 }
}
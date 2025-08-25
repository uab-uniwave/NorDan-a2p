using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Application.Models
{
 public class GlassSapa : BaseGlass
 {
  public string GlassNumber { get; set; } = string.Empty;
  public string TotalAreaPerArticle { get; set; } = string.Empty;
 }

}
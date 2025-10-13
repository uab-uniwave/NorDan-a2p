using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Application.Models
{


 public class MaterialTechnoDesign : BaseMaterial
 {

  public string ArticleType { get; set; } = string.Empty;
  public string SapaArticle { get; set; } = string.Empty;
  public string ColorDescription { get; set; } = string.Empty;
  public string Info { get; set; } = string.Empty;
 }
}

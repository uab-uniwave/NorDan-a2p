using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Application.Models.Models
{
 public class PanelSapa_v2 : BasePanel
 {
  public string ArticleType { get; set; } = string.Empty;
  public string ColorDescription { get; set; } = string.Empty;
  public string SquareMeterPrice { get; set; } = string.Empty;
 }
}
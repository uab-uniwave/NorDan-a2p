using a2p.Application.Importing.BaseModels;

namespace a2p.Application.Importing
{
 public class TechDesignPanelRow : BasePanel
 {
  public string ArticleType { get; set; } = string.Empty;
  public string ColorDescription { get; set; } = string.Empty;
  public string SquareMeterPrice { get; set; } = string.Empty;
 }
}
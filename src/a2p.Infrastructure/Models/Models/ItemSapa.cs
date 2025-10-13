using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Application.Models
{
 public class ItemSapa : BaseItem
 {
  public string MaterialCost { get; set; } = "0";// Direct material cost in local currency
  public string LaborCost { get; set; } = "0"; // Direct labor cost in local currency
  public string LocalPrice { get; set; } = "0"; // Price in local currency
  public string TotalCost { get; set; } = "0"; // Total cost in local currency
  public string PriceEUR { get; set; } = "0";// Price in EUR
  public string TotalCostEUR { get; set; } = "0";// Total cost in EUR
  public string MaterialCostEUR { get; set; } = "0";// Direct material cost in EUR
  public string LaborCostEUR { get; set; } = "0";// Direct labor cost in EUR
 }
}
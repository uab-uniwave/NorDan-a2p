namespace a2p.Shared.Core.Entities.BaseEntities
{
 public class BaseItem
 {
  public string File { get; set; } = string.Empty; // Excel file name
  public string Worksheet { get; set; } = string.Empty;// Worksheet name
  public string Order { get; set; } = string.Empty; // Order identifier
  public string Description { get; set; } = string.Empty; // Item description
  public string SortOrder { get; set; } = "0";// Sorting or ordering index
  public string Item { get; set; } = string.Empty; // Item identifier or name
  public string Quantity { get; set; } = "0";// Quantity of items
  public string Width { get; set; } = "0"; // Width of the item
  public string Height { get; set; } = "0"; // Height of the item
  public string Weight { get; set; } = "0"; // Total weight of the item
  public string WeightConstruction { get; set; } = "0"; // Weight of the construction
  public string WeightGlass { get; set; } = "0"; // Weight of the glass
  public string TotalPrice { get; set; } = "0"; // Calculated: Quantity * Price
 }
}


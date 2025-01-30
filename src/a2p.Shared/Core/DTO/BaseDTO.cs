
using a2p.Shared.Core.Enums;

public class BaseDTO
{
 public string WorksheetName { get; set; } = string.Empty; // WorkSheet name
 public string Order { get; set; } = string.Empty;
 public string Item { get; set; } = string.Empty;
 public string Reference { get; set; } = string.Empty; //SapaArticle or color (SapaV1 full Sheet - Color exists curtting sepc; SapaArticle - no cutting Soec) 
 public string Description { get; set; } = string.Empty; //SapaArticle or color (SapaV1 full Sheet - Color exists curtting sepc; SapaArticle - no cutting Soec) 
 public double Area { get; set; } = 0;
 public decimal Price { get; set; } = 0;
 public int Quantity { get; set; } = 0;
 public decimal TotalPrice { get; set; } = 0;

 public WorksheetType Type { get; set; } = WorksheetType.Unknown;
}
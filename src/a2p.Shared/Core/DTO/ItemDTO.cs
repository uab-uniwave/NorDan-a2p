using a2p.Shared.Core.Enums;

namespace a2p.Shared.Core.DTO
{
 public class ItemDTO
 {

  public string WorksheetName { get; set; } = string.Empty;// WorkSheet name
  public string Order { get; set; } = string.Empty; // From SapaItemV1, Items_Schuco
  public string Project { get; set; } = string.Empty; // From SapaItemV1, Items_Schuco
  public string Item { get; set; } = string.Empty; // From SapaItemV 
  public int SortOrder { get; set; } = 0;// From SapaItemV1, Items_Schuco

  public string Description { get; set; } = string.Empty; // From Items_Schuco

  public int Quantity { get; set; } = 0; // From SapaItemV1, Items_Schuco, Item_Sapa_v2
  public double Width { get; set; } = 0;// From SapaItemV1, Items_Schuco, Item_Sapa_v2
  public double Height { get; set; } = 0; // From SapaItemV1, Items_Schuco, Item_Sapa_v2
  public double Weight { get; set; } = 0;// From SapaItemV1, Items_Schuco
  public double WeightWithoutGlass { get; set; } = 0;// From SapaItemV1 
  public double WeightGlass { get; set; } = 0;// 
  public double WeightTotal { get; set; } = 0;// 
  public double Area { get; set; } = 0; // From SapaItemV1, Items_Schuco, Item_Sapa_v2
  public double TotalArea { get; set; } = 0; // From SapaItemV1, Items_Schuco, Item_Sapa_v2
  public double Hours { get; set; } = 0;

  //Local cost
  //===========================================================
  public decimal MaterialCost { get; set; } = 0; // From SapaItemV1, Item_Sapa_v2 
  public decimal LaborCost { get; set; } = 0; // From SapaItemV1, Items_Schuco, Item_Sapa_v2 (Wages),
  public decimal Cost { get; set; } = 0; // From SapaItemV1, Item_Sapa_v2 SapaPositionsV2 (General Cost) Or From Item_Sapa_v2 (Profiles,Fittings, GasketsAccessories, AluminiumSheet, SetupCostSurfaceTreatment, ClientProfilesAccessories, SortOrder, Panel, SpecialCost, SellingPrice, GeneralCosts, OfferPrice, PriceAdjustage)

  //Price Local and EURO
  //===========================================================
  public decimal Price { get; set; } = 0;// From SapaItemV1, Items_Schuco, Item_Sapa_v2
  public decimal TotalPrice { get; set; } = 0; // From SapaItemV1, Items_Schuco, Item_Sapa_v2

  public string CurrencyCode { get; set; } = string.Empty;
  public decimal EurExchangeRate { get; set; } = 1;

  //EUR cost
  //===========================================================
  public decimal MaterialCostEUR { get; set; } = 0; // From SapaItemV1
  public decimal LaborCostEUR { get; set; } = 0; // From SapaItemV1
  public decimal TotalCostEUR { get; set; } = 0;// From SapaItemV1pub

  public decimal PriceEUR { get; set; } = 0;// From SapaItemV1
  public decimal TotalPriceEUR { get; set; } = 0; // From SapaItemV1

  public WorksheetType WorksheetType { get; set; } = WorksheetType.Unknown;

 }
}
namespace a2p.Shared.Core.Interfaces.Services.Other
{
 public interface IPrefSuiteService
 {
  // string AddItem(Interop.PrefSales.SalesDoc salesDocument, double width, double height, int quantity, double weight, double price, string nomenclature, string model, string description);

  (int, int) GetSalesDocNumber(string order);


 }
}

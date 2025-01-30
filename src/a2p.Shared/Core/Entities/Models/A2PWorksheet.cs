using a2p.Shared.Core.Enums;

namespace a2p.Shared.Core.Entities.Models
{
 public class A2PWorksheet
 {
  public string Order { get; set; } = string.Empty;
  public string Currency { get; set; } = string.Empty;
  public int Items { get; set; } = 0;
  public string FileName { get; set; } = string.Empty;
  public WorksheetType WorksheetType { get; set; } = WorksheetType.Unknown;
  public string Name { get; set; } = string.Empty;
  public int RowCount { get; set; } = 0;
  public List<List<object>> Data { get; set; } = [];

 }

}

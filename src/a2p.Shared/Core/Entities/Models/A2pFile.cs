namespace a2p.Shared.Core.Entities.Models
{
 public class A2PFile
 {
  public string OrderNumber { get; set; } = string.Empty;
  public string File { get; set; } = string.Empty;
  public string Path { get; set; } = string.Empty;

  public string Name { get; set; } = string.Empty;

  public bool IsLocked { get; set; } = false;

  public bool IsOrderFile { get; set; } = false;

  public List<A2PWorksheet> FileWorksheets { get; set; } = [];
 }
}

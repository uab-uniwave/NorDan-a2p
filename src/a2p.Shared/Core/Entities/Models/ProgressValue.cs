namespace a2p.Shared.Core.Entities.Models
{
 public class ProgressValue
 {


  public int MinValue { get; set; } = 0;
  public int MaxValue { get; set; } = 100;


  public int Value { get; set; } = 0;
  public string ProgressTitle { get; set; } = string.Empty;
  public string ProgressTask1 { get; set; } = string.Empty;
  public string ProgressTask2 { get; set; } = string.Empty;

  public string ProgressTask3 { get; set; } = string.Empty;
  public string Order { get; set; } = string.Empty;
  public string WorksheetName { get; set; } = string.Empty;
  public int WorksheetLine { get; set; } = 0;

 }
}

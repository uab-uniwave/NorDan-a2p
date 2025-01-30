namespace a2p.Shared.Core.Entities.Models
{
 public class A2POrder
 {


  public string Number { get; set; } = string.Empty;

  public string Currency { get; set; } = string.Empty;

  public List<A2PFile> Files { get; set; } = [];
 }
}

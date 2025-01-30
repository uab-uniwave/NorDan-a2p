namespace a2p.Shared.Core.Enums
{
 public enum WorksheetType
 {
  Items_Sapa_v1 = 0,  //0 
  Materials_Sapa_v1 = 1,  //1
  Glasses_Sapa_v1 = 2,   //2
  Panels_Sapa_v1 = 3,   //3
  Items_Sapa_v2 = 4,  //4
  Materials_Sapa_v2 = 5,  //5
  Glasses_Sapa_v2 = 6,   //6
  Panels_Sapa_v2 = 7,   //7
  Items_Schuco = 8,  //8
  Materials_Schuco = 9,  //9
  Glasses_Schuco = 10,  //10
  Unknown = 11    //11
 }

 public class Worksheet
 {
  public WorksheetType Type { get; set; } = WorksheetType.Unknown;
 }
}

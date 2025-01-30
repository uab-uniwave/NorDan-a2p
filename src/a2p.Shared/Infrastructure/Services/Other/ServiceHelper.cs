namespace a2p.Shared.Infrastructure.Services.Other
{
 public class ServiceHelper
 {
  public static string GetOrderNumber(string fileName)
  {
   string[] split = fileName.Split('_');
   return split[0];
  }

 }
}

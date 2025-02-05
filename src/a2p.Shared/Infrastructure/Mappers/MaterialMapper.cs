using System.Text.RegularExpressions;

using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Mappers;
using a2p.Shared.Core.Interfaces.Repository.SubSql;
using a2p.Shared.Core.Utils;

namespace a2p.Shared.Infrastructure.Mappers
{
 public class MaterialMapper : IMaterialMapper
 {
  private readonly ILogService _logger;
  private readonly ISqlSapa_v2 _sqlSapa_v2;

  public MaterialMapper(ILogService logger, ISqlSapa_v2 sqlSapa_v2)
  {
   _logger=logger;
   _sqlSapa_v2=sqlSapa_v2;
  }

  // Maps an AppWorksheet to a list of MaterialDTO objects asynchronously
  public async Task<List<MaterialDTO>> MapToMaterialDTOAsync(A2PWorksheet wr)
  {

   string worksheetName = wr.Name;
   string order = wr.Order;
   try
   {
    if (wr==null)
    {
     _logger.Error("Mapping material to materialDTO. AppWorksheet is null");
     return [];

    }
    worksheetName=wr.Name;
    order=wr.Order;

    List<MaterialDTO> materials = [];

    // Determine the type of worksheet and call the appropriate method
    switch (wr.WorksheetType)
    {
     case WorksheetType.Materials_Sapa_v1: // Sapa v1
      _=await GetSapa1Async(wr);
      break;

     case WorksheetType.Materials_Sapa_v2:
      _=await GetSapa2Async(wr);
      if (materials!=null)
      {
       foreach (MaterialDTO material in materials)
       {
        _=await _sqlSapa_v2.ReadMaterialAsync(material);
       }
      }
      break;

     case WorksheetType.Materials_Schuco:
      _=await GetSchucoAsync(wr);
      break;

     default:
      _logger.Error("Mapping material to MaterialDTO. Vendor and/or version unknown!");
      break;
    }
    return materials?? [];
   }

   catch (Exception ex)
   {
    _logger.Error(ex.Message, "Mapping material to MaterialDTO. Unhandled error: Last known Order: {Order},{worksheetName}", new { order, worksheetName });
    return [];
   }


  }

  // Maps a Sapa v1 AppWorksheet to a list of MaterialDTO objects asynchronously
  private async Task<List<MaterialDTO>> GetSapa1Async(A2PWorksheet wr)
  {
   int lineNumber = 0;
   string worksheetName = wr.Name;
   string order = wr.Order;
   try
   {
    if (wr.Data==null)

    {
     _logger.Error("MMDTO Sapa v.1. Worksheet is empty.");
     return [];
    }
    if (wr.Data.Count()==0)
    {
     _logger.Error("MMDTO Sapa v.1. Worksheet is empty.");
     return [];

    }

    List<MaterialDTO> materials = [];
    for (int i = 0; i<wr.RowCount; i++)
    {
     try
     {
      lineNumber=i+1;
      MaterialDTO material = new()
      {
       Type=WorksheetType.Materials_Sapa_v1
      };

      // Add the material to the list
      materials.Add(material);
     }
     catch (Exception ex)
     {
      _logger.Error(ex.Message, "MMDTO Sapa v.1. For each. Unhandled Error. Last known success action. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
      continue;
     }
    }

    return await Task.FromResult(materials);
   }
   catch (Exception ex)
   {
    _logger.Error(ex.Message, "MMDTO Sapa v.1. Unhandled Error. Last known success action. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
    return [];
   }
  }

  // Maps a Sapa v2 AppWorksheet to a list of MaterialDTO objects asynchronously
  private async Task<List<MaterialDTO>> GetSapa2Async(A2PWorksheet wr)
  {

   int lineNumber = 0;

   try
   {



    if (wr==null)
    {

     _logger.Error("MMDTO Sapa v.2. Worksheet is empty OR null");
     return [];
    }


    if (string.IsNullOrEmpty(wr.Name))
    {

     _logger.Error("MMDTO Sapa v.2. Worksheet Name is empty OR null.");
     return [];
    }


    if (string.IsNullOrEmpty(wr.Order))
    {

     _logger.Error("MMDTO Sapa v.2. Worksheet {$Worksheet} Order number is empty OR null", wr.Name);
     return []; ;
    }


    if (wr.RowCount==0)
    {

     _logger.Error("MMDTO Sapa v.2.Error at Order: {$Order}, Worksheet: {$Worksheet). Worksheet has no rows!", wr.Name, wr.Order);
     return []; ;
    }





    List<MaterialDTO> materials = [];
    lineNumber=0;

    return materials=await Task.Run(() =>
    {
     List<MaterialDTO> materials = [];
     for (int i = 4; i<wr.RowCount; i++)
     {
      try
      {


       if (string.IsNullOrEmpty(wr.Name))
       {
        _logger.Error("MMDTO Sapa v.2. WorksheetName is null. Line will be skipped. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}.", wr.Order??string.Empty, wr.Name??string.Empty, lineNumber);
        continue;
       }
       if (string.IsNullOrEmpty(wr.Order))
       {
        _logger.Error("MMDTO Sapa v.2. Order is null. Line will be skipped. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}.", wr.Order??string.Empty, wr.Name??string.Empty, lineNumber);
        continue;
       }



       if (wr.Data.Count==0)
       {
        _logger.Error("MMDTO Sapa v.2.Error at , Order: {$Order}, Worksheet: {$Worksheet). Worksheet has no data!", wr.Order??string.Empty, wr.Name??string.Empty);
        continue;
       }

       if (string.IsNullOrEmpty(wr.Data[i][1].ToString()))
       {
        _logger.Error("MMDTO Sapa v.2. Article field empty. Line will be skipped. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}.", wr.Order??string.Empty, wr.Name??string.Empty, lineNumber);
        continue;
       }

       MaterialDTO material = new()
       {
        WorksheetName=wr.Name??string.Empty,
        Order=wr.Order??string.Empty,




        Reference=wr.Name=="ND_Others"
         ? "ASSA_"+wr.Data[i][1].ToString()??string.Empty
         : GetSapaCode(wr.Data[i][1].ToString()??string.Empty, wr.Data[i][2].ToString()??string.Empty, wr.Order??string.Empty, wr.Name??string.Empty, lineNumber),







        ColorDescription=wr.Data[i][3].ToString()??string.Empty,
        Description=wr.Data[i][4]?.ToString()??string.Empty,

        Quantity=wr.Data[i][5]==null ? 1 : int.TryParse(wr.Data[i][5].ToString(), out int quantity) ? quantity : 1,
        PackageUnit=double.TryParse(wr.Data[i][6].ToString(), out double packageUnit) ? packageUnit : throw new Exception($"SortOrder is not a number. LineNumber: {lineNumber}, Value: {wr.Data[i][6]}"),
        QuantityOrdered=double.TryParse(wr.Data[i][7].ToString(), out double quantityOrdered) ? quantityOrdered : throw new Exception($"QuantityOrdered is not a number. LineNumber: {lineNumber}, Value: {wr.Data[i][7]}"),
        QuantityRequired=double.TryParse(wr.Data[i][8].ToString(), out double quantityRequired) ? quantityRequired : throw new Exception($"QuantityRequired is not a number. LineNumber: {lineNumber}, Value: {wr.Data[i][8]}")
       };

       if (wr.Name=="ND_Profiles")
       {
        material.Waste=double.TryParse(wr.Data[i][9].ToString(), out double waste) ? waste : 0;
        material.Area=double.TryParse(wr.Data[i][10].ToString(), out double area) ? area : 0;
        material.Weight=double.TryParse(wr.Data[i][11].ToString(), out double weight) ? weight : 0;
        material.Price=decimal.TryParse(wr.Data[i][12].ToString(), out decimal price) ? price : 0;
        material.TotalPrice=decimal.TryParse(wr.Data[i][13].ToString(), out decimal totalPrice) ? totalPrice : 0;
       }
       else if (wr.Name is "ND_Others" or "ND_Accessories")
       {
        material.Price=decimal.TryParse(wr.Data[i][9].ToString(), out decimal price) ? price : 0;
        material.TotalPrice=decimal.TryParse(wr.Data[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
       }
       else if ((wr.Name??"Unknown").Trim() is "ND_Gaskets")
       {
        material.Price=decimal.TryParse(wr.Data[i][10].ToString(), out decimal price) ? price : 0;
        material.TotalPrice=decimal.TryParse(wr.Data[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
       }

       material.Type=WorksheetType.Materials_Sapa_v2;

       _logger.Debug("MMDTO Sapa v.2. MATERIAL:"+
        " Order: {$Order} |"+
        " Name: {$WorkSheet} |"+
        " LineNumber: {$Line} |"+
        " Reference: {$Reference} |"+
        " Color: {$Color} |"+
        " ColorDescription: {$ColorDescription} |"+
        " Description: {$Description} |"+
        " Quantity: {$Quantity} |"+
        " PackageUnit: {$PackageUnit} |"+
        " QuantityOrdered: {$QuantityOrdered} |"+
        " QuantityRequired: {$QuantityRequired} |"+
        " Waste: {$Waste} |"+
        " Area: {$Area} |"+
        " Weight: {$Weight} |"+
        " Price: {$Price} |"+
        " TotalPrice: {$TotalPrice} |",

        material.Order,
        material.WorksheetName,
        lineNumber,
        material.Reference,
        material.Color,
        material.ColorDescription,
        material.Description,
        material.Quantity,
        material.PackageUnit,
        material.QuantityOrdered,
        material.QuantityRequired,
        material.Waste,
        material.Area,
        material.Weight,
        material.Price,
        material.TotalPrice
      );
       materials.Add(material);
      }
      catch (Exception ex)
      {
       _logger.Error("MMDTO Sapa v.2. For Each. Unhandled Error. Last known success action. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line} + ${Exception}", wr.Order??string.Empty, wr.Name??string.Empty, lineNumber, ex.Message);
       continue;
      }


     }
     return materials;
    });
   }
   catch (Exception ex)
   {
    _logger.Error("MMDTO Sapa v.2. Unhandled Error. Last known success action. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}+ ${Exception}", wr.Order??string.Empty, wr.Name??string.Empty, lineNumber, ex.Message);
    return [];
   }


  }

  // Maps a Schuco AppWorksheet to a list of MaterialDTO objects asynchronously
  private async Task<List<MaterialDTO>> GetSchucoAsync(A2PWorksheet wr)
  {
   int lineNumber = 0;
   string worksheetName = wr.Name;
   string order = wr.Order;
   try
   {
    if (wr.Data==null)
    {
     _logger.Warning("MMDTO Schuco. AppWorksheet is null. Order: {$Order}", new { order });
     return [];
    }

    if (wr.Data.Count()==0)
    {
     _logger.Warning("MMDTO Schuco. AppWorksheet is empty. Order: {$Order}", new { order });
     return [];
    }



    return await Task.Run(() =>
    {

     List<MaterialDTO> materials = [];

     for (int i = 0; i<wr.RowCount; i++)
     {
      try
      {
       lineNumber=i+1;
       MaterialDTO material = new()
       {
        Type=WorksheetType.Materials_Schuco
       };

       // Add the material to the list
       materials.Add(material);
      }
      catch (Exception ex)
      {
       _logger.Error(ex.Message, "MMDTO Schuco. For each. Unhandled Error. Last known success action. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
       continue;
      }
     }
     return materials;
    });

   }
   catch (Exception ex)
   {
    _logger.Error(ex.Message, "MMDTO Schuco. Last known success action. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
    return [];
   }
  }

  // Generates a Sapa code by merging article and color fields
  private string GetSapaCode(string sapaArticle_v2, string sapaColor_v2, string order, string worksheetName, int lineNumber)
  {
   try
   {


    if (string.IsNullOrEmpty(sapaArticle_v2)&&string.IsNullOrEmpty(sapaColor_v2))
    {
     _logger.Error("MMDTO Sapa v.2. Article and Color fields are empty. Line will be skipped. Order: {$Order}, Name: {$Worksheet}, LineNumber: {$Line}.", order??string.Empty, worksheetName??string.Empty, lineNumber);
    }

    if (string.IsNullOrEmpty(sapaColor_v2)&&(worksheetName=="ND_Gaskets"||worksheetName=="ND_Accessories"))
    {
     if (sapaArticle_v2.StartsWith("S"))
     {
      sapaArticle_v2=sapaArticle_v2[1..];

     }

     return sapaArticle_v2;

    }







    if (worksheetName=="ND_Profiles")
    {

     // Remove 'S' if it is the first character of field1
     if (sapaArticle_v2.StartsWith("S"))
     {
      sapaArticle_v2=sapaArticle_v2[1..];

     }
     // Remove 'S' if it is the first character of field1
     if ((sapaColor_v2.EndsWith("F")||sapaColor_v2.EndsWith("R"))&&sapaColor_v2!="MF")
     {
      sapaColor_v2=sapaColor_v2[..^1];
     }
     _=sapaColor_v2.Replace("A | N", "|N");
    }
    // Merge the fields with a '-'
    string merged = $"{sapaArticle_v2}-{sapaColor_v2}";

    // Regex pattern to keep only letters, numbers, dots, and '-'
    string reference = Regex.Replace(merged, @"[^a-zA-Z0-9.\-|]", "");


    // Ensure the final string is not more than 25 characters
    if (reference.Length>25)
    {
     _logger.Warning("MMDTO Sapa v.2. Generate PrefSuite reference {$Reference}. Length of reference is {$Length} characters.", reference, reference.Length);
     reference=$"*{reference[..24]}";
     _logger.Warning("MMDTO Sapa v.2. Generate PrefSuite reference {$Reference}, trimmed from the end to 25 characters.", reference);
    }
    return reference;
   }
   catch (Exception ex)
   {
    _logger.Error(ex.Message, "MMDTO Sapa v.2. Generate PrefSuite reference using Sapa v.2 article: {$ArticleNumber} and Sapa v.2 Color {$Color}.", new { sapaArticle_v2, sapaColor_v2 });
    return "Unknown";
   }
  }
 }
}
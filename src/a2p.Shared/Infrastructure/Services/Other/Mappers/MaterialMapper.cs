using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;
using a2p.Shared.Core.Interfaces.Services.Other.Mappers;

using System.Text.RegularExpressions;

namespace a2p.Shared.Infrastructure.Mappers
{
    public class MaterialMapper : IMaterialMapper
    {
        private readonly ILogService _logService;


        public MaterialMapper(ILogService logService)
        {
            _logService = logService;

        }


        // Maps a Sapa v1 AppWorksheet to a list of MaterialDTO objects asynchronously
        //=================================================================================
        public async Task<List<MaterialDTO>> GetSapa_v1Async(A2PWorksheet wr)
        {
            int lineNumber = 0;
            string worksheetName = wr.Worksheet;
            string order = wr.OrderNumber;
            try
            {
                if (wr.WorksheetData == null)

                {
                    _logService.Error("MMDTO Sapa v.1. Worksheet is empty.");
                    return [];
                }
                if (wr.WorksheetData.Count() == 0)
                {
                    _logService.Error("MMDTO Sapa v.1. Worksheet is empty.");
                    return [];

                }

                List<MaterialDTO> materials = [];
                for (int i = 0; i < wr.RowCount; i++)
                {
                    try
                    {
                        lineNumber = i + 1;
                        MaterialDTO material = new()
                        {
                            Type = WorksheetType.Materials_Sapa_v1
                        };

                        // Add the material to the list
                        materials.Add(material);
                    }
                    catch (Exception ex)
                    {
                        _logService.Error(ex.Message, "MMDTO Sapa v.1. For each. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                        continue;
                    }
                }

                return await Task.FromResult(materials);
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MMDTO Sapa v.1. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }
        }

        // Maps a Sapa v2 AppWorksheet to a list of MaterialDTO objects asynchronously
        //=================================================================================
        public async Task<List<MaterialDTO>> GetSapa_v2Async(A2PWorksheet wr)
        {

            int lineNumber = 0;

            try
            {



                if (wr == null)
                {

                    _logService.Error("MMDTO Sapa v.2. Worksheet is empty OR null");
                    return [];
                }


                if (string.IsNullOrEmpty(wr.Worksheet))
                {

                    _logService.Error("MMDTO Sapa v.2. Worksheet FileName is empty OR null.");
                    return [];
                }


                if (string.IsNullOrEmpty(wr.OrderNumber))
                {

                    _logService.Error("MMDTO Sapa v.2. Worksheet {$Worksheet} Order number is empty OR null", wr.Worksheet);
                    return []; ;
                }


                if (wr.RowCount == 0)
                {

                    _logService.Error("MMDTO Sapa v.2.Error at Order: {$Order}, Worksheet: {$Worksheet). Worksheet has no rows!", wr.Worksheet, wr.OrderNumber);
                    return []; ;
                }

                List<MaterialDTO> materials = [];
                lineNumber = 0;

                return materials = await Task.Run(() =>
                {
                    List<MaterialDTO> materials = [];
                    for (int i = 4; i < wr.RowCount; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(wr.Worksheet))
                            {
                                _logService.Error("MMDTO Sapa v.2. Worksheet is null. Line will be skipped. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}.", wr.OrderNumber ?? string.Empty, wr.Worksheet ?? string.Empty, lineNumber);
                                continue;
                            }
                            if (string.IsNullOrEmpty(wr.OrderNumber))
                            {
                                _logService.Error("MMDTO Sapa v.2. Order is null. Line will be skipped. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}.", wr.OrderNumber ?? string.Empty, wr.Worksheet ?? string.Empty, lineNumber);
                                continue;
                            }

                            if (wr.WorksheetData.Count == 0)
                            {
                                _logService.Error("MMDTO Sapa v.2.Error at , Order: {$Order}, Worksheet: {$Worksheet). Worksheet has no data!", wr.OrderNumber ?? string.Empty, wr.Worksheet ?? string.Empty);
                                continue;
                            }

                            if (string.IsNullOrEmpty(wr.WorksheetData[i][1].ToString()))
                            {
                                _logService.Error("MMDTO Sapa v.2. Article field empty. Line will be skipped. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}.", wr.OrderNumber ?? string.Empty, wr.Worksheet ?? string.Empty, lineNumber);
                                continue;
                            }

                            MaterialDTO material = new()
                            {
                                Worksheet = wr.Worksheet ?? string.Empty,
                                Order = wr.OrderNumber ?? string.Empty,

                                Reference = wr.Worksheet == "ND_Others"
                                ? "ASSA_" + wr.WorksheetData[i][1].ToString() ?? string.Empty
                                : GetSapa_V2Code(wr.WorksheetData[i][1].ToString() ?? string.Empty, wr.WorksheetData[i][2].ToString() ?? string.Empty, wr.OrderNumber ?? string.Empty, wr.Worksheet ?? string.Empty, lineNumber),
                                SourceReference = wr.WorksheetData[i][1].ToString() ?? string.Empty,
                                SourceColor = wr.WorksheetData[i][2].ToString() ?? string.Empty,
                                ColorDescription = wr.WorksheetData[i][3].ToString() ?? string.Empty,
                                Description = wr.WorksheetData[i][4]?.ToString() ?? string.Empty,
                                Quantity = wr.WorksheetData[i][5] == null ? 1 : int.TryParse(wr.WorksheetData[i][5].ToString(), out int quantity) ? quantity : 1,
                                PackageUnit = double.TryParse(wr.WorksheetData[i][6].ToString(), out double packageUnit) ? packageUnit : throw new Exception($"SortOrder is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][6]}"),
                                QuantityOrdered = double.TryParse(wr.WorksheetData[i][7].ToString(), out double quantityOrdered) ? quantityOrdered : throw new Exception($"QuantityOrdered is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][7]}"),
                                QuantityRequired = double.TryParse(wr.WorksheetData[i][8].ToString(), out double quantityRequired) ? quantityRequired : throw new Exception($"QuantityRequired is not a number. LineNumber: {lineNumber}, Value: {wr.WorksheetData[i][8]}")
                            };
                            if (wr.Worksheet == "ND_Profiles")
                            {
                                material.Waste = double.TryParse(wr.WorksheetData[i][9].ToString(), out double waste) ? waste : 0;
                                material.Area = double.TryParse(wr.WorksheetData[i][10].ToString(), out double area) ? area : 0;
                                material.Weight = double.TryParse(wr.WorksheetData[i][11].ToString(), out double weight) ? weight : 0;
                                material.Price = decimal.TryParse(wr.WorksheetData[i][12].ToString(), out decimal price) ? price : 0;
                                material.TotalPrice = decimal.TryParse(wr.WorksheetData[i][13].ToString(), out decimal totalPrice) ? totalPrice : 0;
                            }
                            else if (wr.Worksheet is "ND_Others" or "ND_Accessories")
                            {
                                material.Price = decimal.TryParse(wr.WorksheetData[i][9].ToString(), out decimal price) ? price : 0;
                                material.TotalPrice = decimal.TryParse(wr.WorksheetData[i][10].ToString(), out decimal totalPrice) ? totalPrice : 0;
                            }
                            else if ((wr.Worksheet ?? "Unknown").Trim() is "ND_Gaskets")
                            {
                                material.Price = decimal.TryParse(wr.WorksheetData[i][10].ToString(), out decimal price) ? price : 0;
                                material.TotalPrice = decimal.TryParse(wr.WorksheetData[i][11].ToString(), out decimal totalPrice) ? totalPrice : 0;
                            }

                            material.Type = WorksheetType.Materials_Sapa_v2;
                            _logService.Debug("MMDTO Sapa v.2. MATERIAL:" +
                                              " Order: {$Order} |" +
                                              " FileName: {$WorkSheet} |" +
                                              " LineNumber: {$Line} |" +
                                              " Reference: {$Reference} |" +
                                              " Color: {$Color} |" +
                                              " ColorDescription: {$ColorDescription} |" +
                                              " Description: {$Description} |" +
                                              " Quantity: {$Quantity} |" +
                                              " PackageUnit: {$PackageUnit} |" +
                                              " QuantityOrdered: {$QuantityOrdered} |" +
                                              " QuantityRequired: {$QuantityRequired} |" +
                                              " Waste: {$Waste} |" +
                                              " Area: {$Area} |" +
                                              " Weight: {$Weight} |" +
                                              " Price: {$Price} |" +
                                              " TotalPrice: {$TotalPrice} |",

                                                 material.Order,
                                                 material.Worksheet,
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
                            _logService.Error("MMDTO Sapa v.2. For Each. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line} + ${Exception}", wr.OrderNumber ?? string.Empty, wr.Worksheet ?? string.Empty, lineNumber, ex.Message);
                            continue;
                        }


                    }
                    return materials;
                });
            }
            catch (Exception ex)
            {
                _logService.Error("MMDTO Sapa v.2. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}+ ${Exception}", wr.OrderNumber ?? string.Empty, wr.Worksheet ?? string.Empty, lineNumber, ex.Message);
                return [];
            }


        }

        // Maps a Schuco AppWorksheet to a list of MaterialDTO objects asynchronously
        //=================================================================================
        public async Task<List<MaterialDTO>> GetSchucoAsync(A2PWorksheet wr)
        {
            int lineNumber = 0;
            string worksheetName = wr.Worksheet;
            string order = wr.OrderNumber;
            try
            {
                if (wr.WorksheetData == null)
                {
                    _logService.Warning("MMDTO Schuco. AppWorksheet is null. Order: {$Order}", new { order });
                    return [];
                }

                if (wr.WorksheetData.Count() == 0)
                {
                    _logService.Warning("MMDTO Schuco. AppWorksheet is empty. Order: {$Order}", new { order });
                    return [];
                }



                return await Task.Run(() =>
                {

                    List<MaterialDTO> materials = [];

                    for (int i = 0; i < wr.RowCount; i++)
                    {
                        try
                        {
                            lineNumber = i + 1;
                            MaterialDTO material = new()
                            {
                                Type = WorksheetType.Materials_Schuco
                            };

                            // Add the material to the list
                            materials.Add(material);
                        }
                        catch (Exception ex)
                        {
                            _logService.Error(ex.Message, "MMDTO Schuco. For each. Unhandled Error. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                            continue;
                        }
                    }
                    return materials;
                });

            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MMDTO Schuco. Last known success action. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}", order, worksheetName, lineNumber);
                return [];
            }
        }

        // Generates a Sapa code by merging article and color fields
        //=================================================================================
        private string GetSapa_V2Code(string sapaArticle_v2, string sapaColor_v2, string order, string worksheetName, int lineNumber)
        {
            try
            {


                if (string.IsNullOrEmpty(sapaArticle_v2) && string.IsNullOrEmpty(sapaColor_v2))
                {
                    _logService.Error("MMDTO Sapa v.2. Article and Color fields are empty. Line will be skipped. Order: {$Order}, FileName: {$Worksheet}, LineNumber: {$Line}.", order ?? string.Empty, worksheetName ?? string.Empty, lineNumber);
                }

                if (string.IsNullOrEmpty(sapaColor_v2) && (worksheetName == "ND_Gaskets" || worksheetName == "ND_Accessories"))
                {
                    if (sapaArticle_v2.StartsWith("S"))
                    {
                        sapaArticle_v2 = sapaArticle_v2[1..];

                    }

                    return sapaArticle_v2;

                }







                if (worksheetName == "ND_Profiles")
                {

                    // Remove 'S' if it is the first character of field1
                    if (sapaArticle_v2.StartsWith("S"))
                    {
                        sapaArticle_v2 = sapaArticle_v2[1..];

                    }
                    // Remove 'S' if it is the first character of field1
                    if ((sapaColor_v2.EndsWith("F") || sapaColor_v2.EndsWith("R")) && sapaColor_v2 != "MF")
                    {
                        sapaColor_v2 = sapaColor_v2[..^1];
                    }
                    _ = sapaColor_v2.Replace("A | N", "|N");
                }
                // Merge the fields with a '-'
                string merged = $"{sapaArticle_v2}-{sapaColor_v2}";

                // Regex pattern to keep only letters, numbers, dots, and '-'
                string reference = Regex.Replace(merged, @"[^a-zA-Z0-9.\-|]", "");


                // Ensure the final string is not more than 25 characters
                if (reference.Length > 25)
                {
                    _logService.Warning("MMDTO Sapa v.2. Generate PrefSuite reference {$Reference}. Length of reference is {$Length} characters.", reference, reference.Length);
                    reference = $"*{reference[..24]}";
                    _logService.Warning("MMDTO Sapa v.2. Generate PrefSuite reference {$Reference}, trimmed from the end to 25 characters.", reference);
                }
                return reference;
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "MMDTO Sapa v.2. Generate PrefSuite reference using Sapa v.2 article: {$ArticleNumber} and Sapa v.2 Color {$Color}.", new { sapaArticle_v2, sapaColor_v2 });
                return "Unknown";
            }
        }
    }
}
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.DTOs;
using a2p.Application.Interfaces;
using a2p.Application.Models;
using a2p.Domain.Enums;

namespace a2p.Infrastructure.Services.PrefSuiteService
{
    public class PrefSuiteService : IPrefSuiteService
    {
        private readonly ILogService _logService;
        private readonly ISQLService _sqlRepository;

        private readonly PrefDataManager.IPrefDataSource _prefSuiteOLEDBConnection;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public PrefSuiteService(ILogService logService, ISQLService sqlRepository)
        {
            _logService = logService;
            _sqlRepository = sqlRepository;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _prefSuiteOLEDBConnection = new PrefDataManager.PrefDataSource();
        }

        public async Task InsertItemsAsync(ExcelOrderDto excelExcelOrderDto, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            try
            {
                _progressValue = progressValue;
                _progress = progress;

                //==============================================================================
                // Insert ItemsDto 
                //==============================================================================
                await Task.Run(() =>
                {
                    PrefSales.SalesDoc salesDoc = new()
                    {
                        ConnectionString = _prefSuiteOLEDBConnection.ConnectionString
                    };

                    _progressValue.CurrentValue += 100; //100  x1 
                    _progressValue.ProgressTask2 = $"Loading PrefSuite sales document ...";
                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);

                    salesDoc.Load(excelExcelOrderDto.SalesDocument.Number, excelExcelOrderDto.SalesDocument.Version);

                    for (int i = 0; i < excelExcelOrderDto.ItemsDto.Count; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(excelExcelOrderDto.ItemsDto[i].ItemName))
                            {
                                continue;
                            }
                            _progressValue.CurrentValue += 10; //10  x2 

                            _progressValue.ProgressTask2 = $"Inserting items {i + 1} of {excelExcelOrderDto.ItemsDto.Count} models into PrefSuite...";
                            _progressValue.ProgressTask3 = $"ItemName # {excelExcelOrderDto.ItemsDto[i].ItemName}";
                            _progress?.Report(_progressValue);


                            string Command = "<cmd:Commands name=\"CommandName\" xmlns:cmd=\"http://www.preference.com/XMLSchemas/2006/PrefCAD.Command\">" +
                                              "<cmd:Command name=\"Model.SetDimensions\">" +
                                              $"<cmd:Parameter name=\"dimensions\" type=\"string\" value=\"W={Math.Ceiling(excelExcelOrderDto.ItemsDto[i].Width)};H={Math.Ceiling(excelExcelOrderDto.ItemsDto[i].Height)};\"/>" +
                                              "</cmd:Command>" +
                                              "<cmd:Command name=\"Model.SetModelVariables\">" +
                                              "<cmd:Parameter name=\"variables\" type=\"list\">" +
                                              "<cmd:ItemName type=\"set\">" +
                                              "<cmd:ItemValue name=\"name\" type=\"string\" value=\"Weight\"/>" +
                                              "<cmd:ItemValue name=\"namespace\" type=\"string\" value=\"\"/>" +
                                              $"<cmd:ItemValue name=\"value\" type=\"real\" value=\"{Math.Round(excelExcelOrderDto.ItemsDto[i].Weight, 4)}\"/>" +
                                              "</cmd:ItemName>" +
                                              "</cmd:Parameter>" +
                                              "</cmd:Command>" +
                                              "<cmd:Command name=\"Model.Regenerate\"/>" +
                                              "</cmd:Commands>";

                            var sdi = salesDoc.Items.Add(excelExcelOrderDto.ItemsDto[i].RowId.ToString());
                            sdi.SetCode("Sapa_ALU", false);
                            sdi.ExecuteCommandStr(Command, out string? resultStr, true);

                            sdi.SetUnitPrice(Math.Round((double)excelExcelOrderDto.ItemsDto[i].Price, 2));
                            sdi.SetUnitCost(Math.Round((double)excelExcelOrderDto.ItemsDto[i].Cost, 2));
                            sdi.PriceClosed = true;
                            sdi.SetQuantity((int)excelExcelOrderDto.ItemsDto[i].Quantity);
                            sdi.Fields["Position"].Value = excelExcelOrderDto.ItemsDto[i].SortOrder.ToString();
                            sdi.Fields["SortOrder"].Value = excelExcelOrderDto.ItemsDto[i].SortOrder.ToString();
                            sdi.Fields["Description"].Value = excelExcelOrderDto.ItemsDto[i].Description;
                            sdi.Fields["Nomenclature"].Value = excelExcelOrderDto.ItemsDto[i].ItemName;

                            _logService.Information($"PrefSuite Service: ItemName {excelExcelOrderDto.ItemsDto[i].ItemName} inserted for excelExcelOrderDto {excelExcelOrderDto.OrderNumber}.");
                        }
                        catch (Exception ex)
                        {
                            _logService.Error(
                                "{$Class}.{$Method}. Unhandled error." +
                                "\nOrder {$OrderNumber}," +
                                "\nWorksheet {$Worksheet}," +
                                "\nLine {$Line}," +
                                "\nItem {ItemName}, " +
                                "\nDescription {Description}," +
                                "\nException: {$Exception}",
                                nameof(PrefSuiteService),
                                nameof(InsertItemsAsync),
                                excelExcelOrderDto.ItemsDto[i].OrderNumber ?? string.Empty,
                                excelExcelOrderDto.ItemsDto[i].Worksheet ?? string.Empty,
                                excelExcelOrderDto.ItemsDto[i].Line,
                                excelExcelOrderDto.ItemsDto[i].ItemName ?? string.Empty,
                                excelExcelOrderDto.ItemsDto[i].Description ?? string.Empty,
                                ex.Message ?? string.Empty
                            );
                            excelExcelOrderDto.ErrorsDto.Add(new ErrorEntity()
                            {
                                OrderNumber = excelExcelOrderDto.ItemsDto[i].OrderNumber ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.SQL_Data_Write,
                                Message = $"{nameof(PrefSuiteService)}.{nameof(InsertItemsAsync)}. Unhandled error." +
                                    $"\nOrder {excelExcelOrderDto.ItemsDto[i].OrderNumber ?? string.Empty}," +
                                    $"\nWorksheet {excelExcelOrderDto.ItemsDto[i].Worksheet ?? string.Empty}," +
                                    $"\nLine {excelExcelOrderDto.ItemsDto[i].Line}," +
                                    $"\nReferenceBase {excelExcelOrderDto.ItemsDto[i].ItemName ?? string.Empty}, " +
                                    $"\nReference {excelExcelOrderDto.ItemsDto[i].Description ?? string.Empty}," +
                                    $"\nException: {ex.Message ?? string.Empty}"
                            });
                            continue;
                        }
                    }

                    _progressValue.CurrentValue += 100; //100  x2 
                    _progressValue.ProgressTask2 = $"Saving PrefSuite sales document ...";
                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                    salesDoc.Save();
                });
                //  return (excelExcelOrderDto, _progressValue);
            }
            catch (Exception ex)
            {
                _logService.Error(
                    "{$Class}.{$Method}. Unhandled error." +
                    "\nOrder {$OrderNumber}," +
                    "\nException: {$Exception}",
                    nameof(PrefSuiteService),
                    nameof(InsertItemsAsync),
                    excelExcelOrderDto.OrderNumber ?? string.Empty,
                    ex.Message ?? string.Empty
                );
                excelExcelOrderDto.ErrorsDto.Add(new ErrorEntity()
                {
                    OrderNumber = excelExcelOrderDto.OrderNumber ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.SQL_Data_Write,
                    Message = $"{nameof(PrefSuiteService)}.{nameof(InsertItemsAsync)}. Unhandled error." +
                        $"\nOrder {excelExcelOrderDto.OrderNumber ?? string.Empty}," +
                        $"\nException: {ex.Message ?? string.Empty}"
                });
                //     return (excelExcelOrderDto, _progressValue);
            }
        }
    }
}


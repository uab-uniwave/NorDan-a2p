// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.DTOs;
using a2p.Application.Interfaces.PrefSuite;
using a2p.Application.Interfaces.Services;
using a2p.Application.Models;
using a2p.Domain.Enums;

using Microsoft.Extensions.Logging;

using PrefSales;
namespace a2p.Infrastructure.Services.PrefSuiteServices
{
    public class PrefSuiteService : IPrefSuiteService
    {
        private readonly ILogger _logger;
        private readonly ISQLService _sqlRepository;

        private readonly PrefDataManager.IPrefDataSource _prefSuiteOLEDBConnection;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public PrefSuiteService(ILogger logger, ISQLService sqlRepository)
        {
            _logger = logger;
            _sqlRepository = sqlRepository;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _prefSuiteOLEDBConnection = new PrefDataManager.PrefDataSource();
        }

        public async Task InsertItemsAsync(OrderDto orderDto, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
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

                    salesDoc.Load(orderDto.SalesDocumentDto.Number, orderDto.SalesDocumentDto.Version);

                    for (int i = 0; i < orderDto.ItemsDto.Count; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(orderDto.ItemsDto[i].ItemName))
                            {
                                continue;
                            }
                            _progressValue.CurrentValue += 10; //10  x2 

                            _progressValue.ProgressTask2 = $"Inserting items {i + 1} of {orderDto.ItemsDto.Count} models into PrefSuite...";
                            _progressValue.ProgressTask3 = $"ItemName # {orderDto.ItemsDto[i].ItemName}";
                            _progress?.Report(_progressValue);

                            string Command = "<cmd:Commands name=\"CommandName\" xmlns:cmd=\"http://www.preference.com/XMLSchemas/2006/PrefCAD.Command\">" +
                                              "<cmd:Command name=\"Model.SetDimensions\">" +
                                              $"<cmd:Parameter name=\"dimensions\" type=\"string\" value=\"W={Math.Ceiling(orderDto.ItemsDto[i].Width)};H={Math.Ceiling(orderDto.ItemsDto[i].Height)};\"/>" +
                                              "</cmd:Command>" +
                                              "<cmd:Command name=\"Model.SetModelVariables\">" +
                                              "<cmd:Parameter name=\"variables\" type=\"list\">" +
                                              "<cmd:ItemName type=\"set\">" +
                                              "<cmd:ItemValue name=\"name\" type=\"string\" value=\"Weight\"/>" +
                                              "<cmd:ItemValue name=\"namespace\" type=\"string\" value=\"\"/>" +
                                              $"<cmd:ItemValue name=\"value\" type=\"real\" value=\"{Math.Round(orderDto.ItemsDto[i].Weight, 4)}\"/>" +
                                              "</cmd:ItemName>" +
                                              "</cmd:Parameter>" +
                                              "</cmd:Command>" +
                                              "<cmd:Command name=\"Model.Regenerate\"/>" +
                                              "</cmd:Commands>";

                            SalesDocItem sdi = salesDoc.Items.Add(orderDto.ItemsDto[i].Id.ToString());
                            sdi.SetCode("Sapa_ALU", false);
                            sdi.ExecuteCommandStr(Command, out string? resultStr, true);

                            sdi.SetUnitPrice(Math.Round((double)orderDto.ItemsDto[i].Price, 2));
                            sdi.SetUnitCost(Math.Round((double)orderDto.ItemsDto[i].Cost, 2));
                            sdi.PriceClosed = true;
                            sdi.SetQuantity((int)orderDto.ItemsDto[i].Quantity);
                            sdi.Fields["Position"].Value = orderDto.ItemsDto[i].SortOrder.ToString();
                            sdi.Fields["SortOrder"].Value = orderDto.ItemsDto[i].SortOrder.ToString();
                            sdi.Fields["Description"].Value = orderDto.ItemsDto[i].Description;
                            sdi.Fields["Nomenclature"].Value = orderDto.ItemsDto[i].ItemName;

                            _logger.LogInformation($"PrefSuite Service: ItemName {orderDto.ItemsDto[i].ItemName} inserted for orderDto {orderDto.OrderNumber}.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                "{$Class}.{$Method}. Unhandled error." +
                                "\nOrder {$OrderNumber}," +
                                "\nWorksheet {$WorksheetDto}," +
                                "\nLine {$Line}," +
                                "\nItem {ItemName}, " +
                                "\nDescription {Description}," +
                                "\nException: {$Exception}",
                                nameof(PrefSuiteService),
                                nameof(InsertItemsAsync),
                                orderDto.ItemsDto[i].OrderNumber ?? string.Empty,
                                orderDto.ItemsDto[i].Worksheet ?? string.Empty,
                                orderDto.ItemsDto[i].Line,
                                orderDto.ItemsDto[i].ItemName ?? string.Empty,
                                orderDto.ItemsDto[i].Description ?? string.Empty,
                                ex.Message ?? string.Empty
                            );
                            orderDto.ErrorsDto.Add(new ErrorDto()
                            {
                                OrderNumber = orderDto.ItemsDto[i].OrderNumber ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.SQL_Data_Write,
                                Message = $"{nameof(PrefSuiteService)}.{nameof(InsertItemsAsync)}. Unhandled error." +
                                    $"\nOrder {orderDto.ItemsDto[i].OrderNumber ?? string.Empty}," +
                                    $"\nWorksheet {orderDto.ItemsDto[i].Worksheet ?? string.Empty}," +
                                    $"\nLine {orderDto.ItemsDto[i].Line}," +
                                    $"\nReferenceBase {orderDto.ItemsDto[i].ItemName ?? string.Empty}, " +
                                    $"\nReference {orderDto.ItemsDto[i].Description ?? string.Empty}," +
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
                //  return (orderDto, _progressValue);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "{$Class}.{$Method}. Unhandled error." +
                    "\nOrder {$OrderNumber}," +
                    "\nException: {$Exception}",
                    nameof(PrefSuiteService),
                    nameof(InsertItemsAsync),
                    orderDto.OrderNumber ?? string.Empty,
                    ex.Message ?? string.Empty
                );
                orderDto.ErrorsDto.Add(new ErrorDto()
                {
                    OrderNumber = orderDto.OrderNumber ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.SQL_Data_Write,
                    Message = $"{nameof(PrefSuiteService)}.{nameof(InsertItemsAsync)}. Unhandled error." +
                        $"\nOrder {orderDto.OrderNumber ?? string.Empty}," +
                        $"\nException: {ex.Message ?? string.Empty}"
                });
                //     return (orderDto, _progressValue);
            }
        }
    }
}


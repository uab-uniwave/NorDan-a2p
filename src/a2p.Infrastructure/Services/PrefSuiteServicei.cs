// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Services;
using a2p.Domain.Entities;
using a2p.Domain.Enums;
using a2p.Domain.Models;

namespace a2p.Infrastructure.Services
{
    public class PrefSuiteService : IPrefSuiteService
    {
        private readonly ILogService _logService;
        private readonly ISQLService _sqlRepository;

        private readonly Interop.PrefDataManager.IPrefDataSource _prefSuiteOLEDBConnection;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public PrefSuiteService(ILogService logService, ISQLService sqlRepository)
        {
            _logService = logService;
            _sqlRepository = sqlRepository;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _prefSuiteOLEDBConnection = new Interop.PrefDataManager.PrefDataSource();
        }

        public async Task<(OrderEntity, ProgressValue)> InsertItemsAsync(OrderEntity order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {



            try
            {

                _progressValue = progressValue;
                _progress = progress;



                //==============================================================================
                // Insert Items 
                //==============================================================================
                await Task.Run(() =>
                {
                    Interop.PrefSales.SalesDoc salesDoc = new()
                    {
                        ConnectionString = _prefSuiteOLEDBConnection.ConnectionString
                    };

                    _progressValue.CurrentValue = _progressValue.CurrentValue + 100; //100  x1 
                    _progressValue.ProgressTask2 = $"Loading PrefSuite sales document ...";
                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);


                    salesDoc.Load(order.SalesDocumentNumber, order.SalesDocumentVersion);

                    for (int i = 0; i < order.Items.Count; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(order.Items[i].ItemName))

                            {
                                continue;
                            }
                            _progressValue.CurrentValue = _progressValue.CurrentValue + 10; //10  x2 

                            _progressValue.ProgressTask2 = $"Inserting items {i + 1} of {order.Items.Count} models into PrefSuite...";
                            _progressValue.ProgressTask3 = $"Item # {order.Items[i].ItemName}";
                            _progress?.Report(_progressValue);

                            string idPos = Guid.NewGuid().ToString();




                            string Command = "<cmd:Commands name=\"CommandName\" xmlns:cmd=\"http://www.preference.com/XMLSchemas/2006/PrefCAD.Command\">" +
                                                                  "<cmd:Command name=\"Model.SetDimensions\">" +
                                                                      $"<cmd:Parameter name=\"dimensions\" type=\"string\" value=\"W={Math.Ceiling(order.Items[i].Width)};H={Math.Ceiling(order.Items[i].Height)};\"/>" +
                                                                  "</cmd:Command>" +
                                                                  "<cmd:Command name=\"Model.SetModelVariables\">" +
                                                                      "<cmd:Parameter name=\"variables\" type=\"list\">" +
                                                                      "<cmd:Item type=\"set\">" +
                                                                      "<cmd:ItemValue name=\"name\" type=\"string\" value=\"Weight\"/>" +
                                                                      "<cmd:ItemValue name=\"namespace\" type=\"string\" value=\"\"/>" +
                                                                     $"<cmd:ItemValue name=\"value\" type=\"real\" value=\"{Math.Round(order.Items[i].Weight, 4)}\"/>" +
                                                                      "</cmd:Item>" +
                                                                       "</cmd:Parameter>" +
                                                                  "</cmd:Command>" +
                                                                  "<cmd:Command name=\"Model.Regenerate\"/>" +
                                                                  "</cmd:Commands>";



                            var sdi = salesDoc.Items.Add(idPos);
                            sdi.SetCode("Sapa_ALU", false);
                            sdi.ExecuteCommandStr(Command, out string? resultStr, true);


                            sdi.SetUnitPrice(Math.Round((double)order.Items[i].Price, 2));
                            sdi.SetUnitCost(Math.Round((double)order.Items[i].Cost, 2));
                            sdi.PriceClosed = true;
                            sdi.SetQuantity((int)order.Items[i].Quantity);
                            sdi.Fields["Position"].Value = order.Items[i].SortOrder.ToString();
                            sdi.Fields["SortOrder"].Value = order.Items[i].SortOrder.ToString();
                            sdi.Fields["Description"].Value = order.Items[i].Description;
                            sdi.Fields["Nomenclature"].Value = order.Items[i].ItemName.ToString();

                            order.Items[i].SalesDocumentIdPos = idPos;




                            _logService.Information($"PrefSuite Service: Item {order.Items[i].ItemName} inserted for order {order.OrderNumber}.");
                        }
                        catch (Exception ex)
                        {
                            _logService.Error(
                            "{$Class}.{$Method}. Unhandled error." +
                            "\nOrder {$Order}," +
                            "\nWorksheet {$Worksheet}," +
                            "\nLine {$Line}," +
                            "\nItem {Item}, " +
                            "\nDescription {Description}," +
                            "\nException: {$Exception}",
                            nameof(PrefSuiteService),
                            nameof(InsertItemsAsync),
                            order.Items[i].Order ?? string.Empty,
                            order.Items[i].Worksheet ?? string.Empty,
                            order.Items[i].Line,
                           order.Items[i].ItemName ?? string.Empty,
                            order.Items[i].Description ?? string.Empty,
                            ex.Message ?? string.Empty
                           );
                            order.Errors.Add(new ErrorEntity()
                            {
                                OrderNumber = order.Items[i].Order ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.DatabaseWrite_Material,
                                Message = $"{nameof(PrefSuiteService)}.{nameof(InsertItemsAsync)}. Unhandled error." +
                               $"\nOrder {order.Items[i].Order ?? string.Empty}," +
                               $"\nWorksheet {order.Items[i].Worksheet ?? string.Empty}," +
                               $"\nLine {order.Items[i].Line}," +
                               $"\nReferenceBase {order.Items[i].ItemName ?? string.Empty}, " +
                               $"\nReference {order.Items[i].Description ?? string.Empty}," +
                               $"\nException: {ex.Message ?? string.Empty}"
                            });
                            continue;
                        }

                    }

                    _progressValue.CurrentValue = _progressValue.CurrentValue + 100; //100  x2 
                    _progressValue.ProgressTask2 = $"Saving PrefSuite sales document ...";
                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);
                    salesDoc.Save();

                });
                return (order, _progressValue);

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nException: {$Exception}",
                nameof(PrefSuiteService),
                nameof(InsertItemsAsync),
                order.OrderNumber ?? string.Empty,

                ex.Message ?? string.Empty
               );
                order.Errors.Add(new ErrorEntity()
                {
                    OrderNumber = order.OrderNumber ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(PrefSuiteService)}.{nameof(InsertItemsAsync)}. Unhandled error." +
                   $"\nOrder {order.OrderNumber ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"
                });
                return (order, _progressValue);
            }

        }

    }
}


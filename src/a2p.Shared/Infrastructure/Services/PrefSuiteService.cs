// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace a2p.Shared.Infrastructure.Services
{
    public class PrefSuiteService : IPrefSuiteService
    {
        private readonly ILogService _logService;
        private readonly ISqlRepository _sqlRepository;

        private readonly DataCache _dataCache;
        private readonly IConfiguration _configuration;
        private readonly Interop.PrefDataManager.IPrefDataSource _prefSuiteOLEDBConnection;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;
        private int _errorCount = 0;
        private int _warningCount = 0;

        public PrefSuiteService(ILogService logService, ISqlRepository sqlRepository, IConfiguration configuration, DataCache dataCache)
        {
            _logService = logService;
            _sqlRepository = sqlRepository;
            _configuration = configuration;
            _dataCache = dataCache;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

            _prefSuiteOLEDBConnection = new Interop.PrefDataManager.PrefDataSource();
        }

        public async Task GetSalesDocumentStates(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();

            List<A2POrder> a2pOrders = _dataCache.GetAllOrders();
            _progressValue.MinValue = 0;
            _progressValue.MaxValue = (a2pOrders.Count * 2) + 3;
            _progressValue.Value = 0;
            _progressValue.ProgressTitle = $"Checking orders errors'";
            _progressValue.ProgressTask1 = string.Empty;
            _progressValue.ProgressTask2 = string.Empty;
            _progressValue.ProgressTask3 = string.Empty;

            foreach (A2POrder a2pOrder in a2pOrders)
            {
                {
                    _warningCount += a2pOrder.ReadErrors
                                 .Where(error => error.Level is ErrorLevel.Warning)
                                 .Select(error => new { a2pOrder.Order, error.Level, error.Code, error.Message })
                                 .Distinct()
                                 .Count();
                    _errorCount += a2pOrder.ReadErrors
                                .Where(error => error.Level is ErrorLevel.Error or ErrorLevel.Fatal)
                                .Select(error => new { error.Level, error.Code, error.Message })
                                .Distinct()
                                .Count();
                }

                int ordersCounter = 0;
                int progressBarValue = 0;

                foreach (A2POrder a2pOrder2 in a2pOrders)
                {
                    progressBarValue++;

                    ordersCounter++;

                    _progressValue.Value = progressBarValue;
                    _progressValue.ProgressTask1 = $"Order {ordersCounter} of {a2pOrders.Count} - Order {a2pOrder2.Order}";
                    _progress?.Report(_progressValue);

                    SqlCommand cmd = new()
                    {
                        CommandText = "SELECT [dbo].[Uniwave_a2p_GetOrderState](@Order)",
                        CommandType = CommandType.Text,
                    };

                    _ = cmd.Parameters.AddWithValue("@Order", a2pOrder2.Order);
                    try
                    {
                        try
                        {
                            object? result = await _sqlRepository.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                            int state = result != DBNull.Value ? (int)result : default;
                            _dataCache.UpdateOrderInCache(a2pOrder2.Order, a2pOrder2 => a2pOrder2.SalesDocumentState = state);

                            await SetSalesDocumentReadErrors(progressValue, _progress);
                        }
                        catch (Exception ex)
                        {
                            _logService.Error("PrefSuite service: Error getting sales document status for {$Order}. Exception {$Exception}", a2pOrder2.SalesDocumentState, ex.Message);
                        }

                        await SetSalesDocumentReadErrors(progressValue, _progress);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("PrefSuite service: Error getting sales document status for {$Order}. Exception {$Exception}", a2pOrder2.SalesDocumentState, ex.Message);

                    }
                    progressBarValue++;
                    _progressValue.Value = progressBarValue;
                    _progress?.Report(_progressValue);

                }
            }

        }

        private async Task SetSalesDocumentReadErrors(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();
            try
            {
                int ordersCounter = 0;
                List<A2POrder> a2pOrders = _dataCache.GetAllOrders();

                foreach (A2POrder a2pOrder in a2pOrders)
                {
                    ordersCounter++;
                    _progressValue.ProgressTitle = $"Checkinhg Errors in Order # {a2pOrder.Order}. {ordersCounter} of {a2pOrders.Count}";
                    _progress?.Report(_progressValue);

                    _progressValue.ProgressTitle = $"Reading Order States # {a2pOrder.Order}. {ordersCounter} of {a2pOrders.Count}";
                    _progress?.Report(_progressValue);

                    OrderState orderState = (OrderState)a2pOrder.SalesDocumentState;

                    if (orderState.HasFlag(OrderState.SalesDocumentExist))
                    {
                        string sqlCommand = "SELECT Numero, Version FROM PAF WHERE Referencia = " + "'" + a2pOrder.Order + "'";
                        CommandType commandType = System.Data.CommandType.Text;
                        (int, int) result = await _sqlRepository.ExecuteQueryTupleValuesAsync(sqlCommand, commandType);
                        if (result.Item1 > 0 && result.Item2 > 0)
                        {
                            _dataCache.UpdateOrderInCache(a2pOrder.Order, a2pOrder =>
                            {
                                a2pOrder.SalesDocumentNumber = result.Item1;
                                a2pOrder.SalesDocumentVersion = result.Item2;
                            });

                        }

                        else
                        {
                            a2pOrder.ReadErrors.Add(new A2POrderError
                            {
                                Order = a2pOrder.Order,
                                Level = ErrorLevel.Fatal,
                                Code = ErrorCode.DatabaseRead_OrderReferenceNotFound,
                                Message = $"PrefSuite sales document with reference {a2pOrder.Order} not exist."

                            });
                            _errorCount++;

                            progressValue.ProgressTask2 = $"Errors: {_errorCount}, Warnings: {_warningCount}";
                            progress?.Report(progressValue);
                        }
                    }

                    if (orderState.HasFlag(OrderState.A2PItemsImported))
                    {
                        a2pOrder.ReadErrors.Add(new A2POrderError
                        {
                            Order = a2pOrder.Order,
                            Level = ErrorLevel.Warning,
                            Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                            Message = $"Order {a2pOrder.Order} items has already been imported to DB."

                        });

                        _warningCount++;

                        progressValue.ProgressTask2 = $"Errors: {_errorCount}, Warnings: {_warningCount}";
                        progress?.Report(progressValue);

                    }

                    if (orderState.HasFlag(OrderState.A2PMaterialsImported))
                    {
                        a2pOrder.ReadErrors.Add(new A2POrderError
                        {
                            Order = a2pOrder.Order,
                            Level = ErrorLevel.Warning,
                            Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                            Message = $"Order {a2pOrder.Order} materials has already been imported toDB ."

                        });

                        _warningCount++;

                        progressValue.ProgressTask2 = $"Errors: {_errorCount}, Warnings: {_warningCount}";
                        progress?.Report(progressValue);

                    }
                    if (orderState.HasFlag(OrderState.ItemsCreated))
                    {
                        a2pOrder.ReadErrors.Add(new A2POrderError
                        {
                            Order = a2pOrder.Order,
                            Level = ErrorLevel.Warning,
                            Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                            Message = $"Order {a2pOrder.Order} PrefSuite Items are Created"

                        });

                        _warningCount++;

                        progressValue.ProgressTask2 = $"Errors: {_errorCount}, Warnings: {_warningCount}";
                        progress?.Report(progressValue);
                    }

                    if (orderState.HasFlag(OrderState.MaterialNeedsInserted))
                    {
                        a2pOrder.ReadErrors.Add(new A2POrderError
                        {
                            Order = a2pOrder.Order,
                            Level = ErrorLevel.Warning,
                            Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                            Message = $"Order {a2pOrder.Order} PrefSuite Material Needs already exist."

                        });

                        _warningCount++;

                        progressValue.ProgressTask2 = $"Errors: {_errorCount}, Warnings: {_warningCount}";
                        progress?.Report(progressValue);
                    }

                    if (orderState.HasFlag(OrderState.PurchaseOrdersExist))
                    {
                        a2pOrder.ReadErrors.Add(new A2POrderError
                        {
                            Order = a2pOrder.Order,
                            Level = ErrorLevel.Fatal,
                            Code = ErrorCode.DatabaseRead_OrderAlreadyImported,
                            Message = $"Order {a2pOrder.Order} PrefSuite Material Needs already exist."

                        });
                        _errorCount++;

                        progressValue.ProgressTask2 = $"Errors: {_errorCount}, Warnings: {_warningCount}";
                        progress?.Report(progressValue);
                    }

                }

            }
            catch (Exception ex)
            {
                _logService.Error("PrefSuite Service: Unhandled error reading orders States. Exception {$Exception}", ex.Message);

            }

        }

        public async Task<string?> GetColorAsync(string color)
        {
            try
            {
                string sqlCommand = "SELECT Color FROM Colors WHERE Color = '" + color + "'";
                CommandType commandType = System.Data.CommandType.Text;
                object? result = await _sqlRepository.ExecuteScalarAsync(sqlCommand, commandType);

                return result?.ToString();
            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: getting color from DB");
                return null;
            }
        }

        public async Task InsertItemsAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            try
            {
                _progressValue = progressValue;
                _progress = progress ?? new Progress<ProgressValue>();

                List<A2POrder> a2pOrders = _dataCache.GetAllOrders();

                for (int j = 0; j < a2pOrders.Count; j++)
                {

                    try
                    {
                        if (a2pOrders[j].Items.Any())
                        {
                            Interop.PrefSales.SalesDoc salesDoc = new()
                            {
                                ConnectionString = _prefSuiteOLEDBConnection.ConnectionString
                            };

                            _progressValue.ProgressTask2 = $"Loading PrefSuite Sales Document {a2pOrders[j].SalesDocumentNumber}/{a2pOrders[j].SalesDocumentVersion}";
                            _progress?.Report(progressValue);
                            salesDoc.Load(a2pOrders[j].SalesDocumentNumber, a2pOrders[j].SalesDocumentVersion);
                            for (int i = 0; i < a2pOrders[j].Items.Count; i++)
                            {
                                _progressValue.ProgressTask3 = _warningCount > 0 || _errorCount > 0 ? $"Errors: {_errorCount}, Warnings: {_warningCount}" : string.Empty;
                                _progressValue.ProgressTask2 = $"Inserting item into PrefSuite {i + 1} of {a2pOrders[j].Items.Count} - Item {a2pOrders[j].Items[i].Item}";
                                _progress?.Report(progressValue);

                                try
                                {
                                    string Command =
                                        "<cmd:Commands name=\"CommandName\" xmlns:cmd=\"http://www.preference.com/XMLSchemas/2006/PrefCAD.Command\">" +
                                        "<cmd:Command name=\"Model.SetDimensions\">" +
                                        "<cmd:Parameter name=\"dimensions\" type=\"string\" " +
                                        $"value=\"W= {a2pOrders[j].Items[i].Width};H= {a2pOrders[j].Items[i].Height};\"/>" +
                                        "</cmd:Command>" +
                                        "<cmd:Command name=\"Model.SetModelVariables\">" +
                                        "<cmd:Parameter name=\"variables\" type=\"list\">" +
                                        "<cmd:Item type=\"set\">" +
                                        "<cmd:ItemValue name=\"name\" type=\"string\" value=\"Weight\"/>" +
                                        "<cmd:ItemValue name=\"namespace\" type=\"string\" value=\"\"/>" +
                                        $"<cmd:ItemValue name=\"value\" type=\"real\" value=\"{a2pOrders[j].Items[i].Weight}\"/>" +
                                        "</cmd:Item> </cmd:Parameter></cmd:Command>" +
                                        "<cmd:Command name=\"Model.Regenerate\"/>" +
                                        "</cmd:Commands>";

                                    string idPos = Guid.NewGuid().ToString();

                                    await Task.Run(() =>
                                    {
                                        Interop.PrefSales.SalesDocItem sdi = salesDoc.Items.Add(idPos);
                                        sdi.SetCode("ALU_SAPA", false);
                                        _ = sdi.ExecuteCommandStr(Command, out string? resultStr, true);
                                        sdi.SetUnitPrice((double)a2pOrders[j].Items[i].Price);
                                        sdi.SetUnitCost((double)a2pOrders[j].Items[i].Cost);
                                        sdi.PriceClosed = true;
                                        sdi.SetQuantity(a2pOrders[j].Items[i].Quantity);
                                        sdi.Fields["Position"].Value = a2pOrders[j].Items[i].SortOrder.ToString();
                                        sdi.Fields["SortOrder"].Value = a2pOrders[j].Items[i].SortOrder.ToString();
                                        sdi.Fields["Description"].Value = a2pOrders[j].Items[i].Description;
                                    });

                                    a2pOrders[j].Items[i].SalesDocumentIdPos = idPos;
                                    _dataCache.UpdateOrderInCache(a2pOrders[j].Order, a2pOrder => a2pOrder.Items[i].SalesDocumentIdPos = idPos);
                                }
                                catch (Exception ex)
                                {
                                    _logService.Error("PrefSuite Service: Error inserting items for order {$Order}. Exception {$Exception}", a2pOrders[j].Order, ex.Message);
                                    a2pOrders[j].WriteErrors.Add(new A2POrderError
                                    {
                                        Order = a2pOrders[j].Order,
                                        Level = ErrorLevel.Error,
                                        Code = ErrorCode.ERPWrite_Item,
                                        Message = $"Order {a2pOrders[j].Order}. Error inserting items into PrefSuite. Exception {ex.Message}"
                                    });
                                    _errorCount++;

                                    _progressValue.ProgressTask3 = _warningCount > 0 || _errorCount > 0 ? $"Errors: {_errorCount}, Warnings: {_warningCount}" : string.Empty;
                                    _progressValue.ProgressTask2 = $"Saving PrefSuite Sales Document {a2pOrders[j].SalesDocumentNumber}/{a2pOrders[j].SalesDocumentVersion}.";
                                    _progress?.Report(progressValue);
                                    salesDoc.Save();

                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        _logService.Error("PrefSuite Service: Error inserting items for order {$Order}. Exception {$Exception}", a2pOrders[j].Order, ex.Message);
                        a2pOrders[j].WriteErrors.Add(new A2POrderError
                        {
                            Order = a2pOrders[j].Order,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.ERPWrite_Item,
                            Message = $"PrefSuite Service: Error inserting items for order {a2pOrders[j].Order}. Exception {ex.Message}",
                        });
                        _errorCount++;
                        _progressValue.ProgressTask3 = $"Errors: {_errorCount}, Warnings: {_warningCount}";
                        _progress?.Report(progressValue);

                    }
                }
            }
            catch (Exception ex)
            {
                _logService.Error("PrefSuite Service: Unhandled error inserting items. Exception {$Exception}", ex.Message);

            }
        }
    }
}


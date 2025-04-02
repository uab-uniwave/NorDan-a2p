// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Data.SqlClient;

namespace a2p.Shared.Infrastructure.Services
{
    public class PrefSuiteService : IPrefSuiteService
    {
        private readonly ILogService _logService;
        private readonly ISqlRepository _sqlRepository;

        private readonly DataCache _dataCache;
        private readonly Interop.PrefDataManager.IPrefDataSource _prefSuiteOLEDBConnection;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public PrefSuiteService(ILogService logService, ISqlRepository sqlRepository, DataCache dataCache)
        {
            _logService = logService;
            _sqlRepository = sqlRepository;
            _dataCache = dataCache;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

            _prefSuiteOLEDBConnection = new Interop.PrefDataManager.PrefDataSource();
        }

        public async Task<ProgressValue> GetSalesDocumentStates(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            try
            {

                _progressValue = progressValue;
                _progress = progress ?? new Progress<ProgressValue>();

                List<A2POrder> a2pOrders = _dataCache.GetAllOrders();

                _progressValue.ProgressTitle = $"Get Sales Documents States'";
                int ordersCounter = 0;
                foreach (A2POrder a2pOrder in a2pOrders)
                {

                    ordersCounter++;
                    _progressValue.Value++; // Increment the progress value - Read per Order 4 
                    _progressValue.ProgressTask1 = $"Order {ordersCounter} of {a2pOrders.Count} - Order #{a2pOrder.Order}";
                    _progress?.Report(_progressValue);

                    SqlCommand cmd = new()
                    {
                        CommandText = "SELECT [dbo].[Uniwave_a2p_GetOrderState](@Order)",
                        CommandType = CommandType.Text,
                    };

                    _ = cmd.Parameters.AddWithValue("@Order", a2pOrder.Order);

                    object? result = await _sqlRepository.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                    int state = result != DBNull.Value ? (int)result! : 0;

                    _progressValue.Value++; // Increment the progress value - Read per Order 5
                    _progress?.Report(_progressValue);
                    _dataCache.UpdateOrderInCache(a2pOrder.Order, a2pOrder2 => a2pOrder.SalesDocumentState = state);

                    _progressValue.Value++; // Increment the progress value - Read per Order 6
                    _progress?.Report(_progressValue);
                    await SetSalesDocumentReadErrors(progressValue, _progress);

                    _progressValue.Value++; // Increment the progress value - Read per Order 6
                    _progress?.Report(_progressValue);
                }
                return _progressValue;
            }
            catch (Exception ex)
            {

                _logService.Error("PrefSuite service: Unhandled error getting sales document status. Exception {$Exception}", ex.Message);
                return _progressValue;
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
                    _progressValue.ProgressTitle = $"Checking Errors. {ordersCounter} of {a2pOrders.Count} -  Order # {a2pOrder.Order}.";
                    _progress?.Report(_progressValue);

                    _progressValue.ProgressTitle = $"Reading Order States # {ordersCounter} of {a2pOrders.Count} - Order # {a2pOrder.Order}."; _progress?.Report(_progressValue);
                    string sqlCommand = "SELECT Numero, Version FROM PAF WHERE Referencia = " + "'" + a2pOrder.Order + "'";

                    CommandType commandType = CommandType.Text;
                    (int, int) result = await _sqlRepository.ExecuteQueryTupleValuesAsync(sqlCommand, commandType);
                    OrderState orderState = (OrderState)a2pOrder.SalesDocumentState;
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

                    }

                }

            }
            catch (Exception ex)
            {
                _logService.Error("PrefSuite Service: Unhandled error reading orders States. Exception {$Exception}", ex.Message);

            }

            finally
            {

                _progress?.Report(_progressValue);
            }
        }

        public async Task<string?> GetColorAsync(string color)
        {
            try
            {
                string sqlCommand = $"SELECT Color FROM Colors WHERE Color = '{color}'";
                CommandType commandType = CommandType.Text;
                object? result = await _sqlRepository.ExecuteScalarAsync(sqlCommand, commandType);

                return result != null ? (result?.ToString()) : null;

            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: getting color from DB");
                return null;
            }
        }

        public async Task<string?> GetGlassReferenceAsync(string description)
        {
            try
            {
                string sqlCommand = $"SELECT TOP 1 ReferenciaBase FROM MaterialesBase WHERE tipocalculo = 'Superficies' and Nivel1 = '03 Glass' and Descripcion = '{description}'";
                CommandType commandType = CommandType.Text;
                object? result = await _sqlRepository.ExecuteScalarAsync(sqlCommand, commandType);
                return result?.ToString();

            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: getting color from DB");
                return null;
            }
        }
        public async Task<ProgressValue> InsertItemsAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();

            try
            {

                await Task.Run(() =>
                {
                    Interop.PrefSales.SalesDoc salesDoc = new()
                    {
                        ConnectionString = _prefSuiteOLEDBConnection.ConnectionString
                    };
                    List<A2POrder> a2pOrders = _dataCache.GetAllOrders();
                    int ordersToProcessCount = a2pOrders.Select(x => x.Import == true).Count();
                    int ordersCounter = 0;
                    for (int j = 0; j < a2pOrders.Count; j++)
                    {

                        try

                        {
                            if (a2pOrders[j].Items.Any() && a2pOrders[j].Import == true)
                            {
                                ordersCounter++;
                                _progressValue.ProgressTask1 = $"Inserting PrefSuite orders {j + 1} of {ordersToProcessCount} - Order # {a2pOrders[j].Order} {a2pOrders[j].SalesDocumentNumber}/{a2pOrders[j].SalesDocumentVersion}";

                                _progressValue.ProgressTask2 = string.Empty;
                                _progressValue.ProgressTask3 = string.Empty;

                                _progress?.Report(progressValue);

                                //==============================================================================
                                // Insert Items 
                                //==============================================================================
                                for (int i = 0; i < a2pOrders[j].Items.Count; i++)
                                {
                                    try
                                    {
                                        salesDoc.Load(a2pOrders[j].SalesDocumentNumber, a2pOrders[j].SalesDocumentVersion);
                                        string idPos = Guid.NewGuid().ToString();
                                        _progressValue.ProgressTask2 = $"Inserting PrefSuite items. Item {i + 1} of {a2pOrders[j].Items.Count} - Item # {a2pOrders[j].Items[i].Item}";
                                        _progress?.Report(progressValue);
                                        string Command =
                                            $"<cmd:Commands name=\"CommandName\" xmlns:cmd=\"http://www.preference.com/XMLSchemas/2006/PrefCAD.Command\">" +
                                                $"<cmd:Command name=\"Model.SetDimensions\">" +
                                                   $"<cmd:Parameter name=\"dimensions\" type=\"string\" value=\"W= {a2pOrders[j].Items[i].Width};H= {a2pOrders[j].Items[i].Height}\"/>" +
                                                $"</cmd:Command>" +
                                                $"<cmd:Command name=\"Model.SetModelVariables\">" +
                                                    $"<cmd:Parameter name=\"variables\" type=\"list\">" +
                                                        $"<cmd:Item type=\"set\">" +
                                                            $"<cmd:ItemValue name=\"name\" type=\"string\" value=\"Weight\"/>" +
                                                            $"<cmd:ItemValue name=\"namespace\" type=\"string\" value=\"\"/>" +
                                                            $"<cmd:ItemValue name=\"value\" type=\"real\" value=\"{Math.Round(a2pOrders[j].Items[i].Weight, 2)}\"/>" +
                                                        $"</cmd:Item> " +
                                                    $"</cmd:Parameter></cmd:Command>" +
                                                $"<cmd:Command name=\"Model.Regenerate\"/>" +
                                            $"</cmd:Commands>";

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
                                        a2pOrders[j].Items[i].SalesDocumentIdPos = idPos;
                                        _dataCache.UpdateOrderInCache(a2pOrders[j].Order, a2pOrder => a2pOrder.Items[i].SalesDocumentIdPos = idPos);
                                        salesDoc.Save();

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

                                        _progressValue.ProgressTask1 = $"Saving PrefSuite Sales Document {a2pOrders[j].SalesDocumentNumber}/{a2pOrders[j].SalesDocumentVersion}.";
                                        _progress?.Report(progressValue);
                                        continue;

                                    }

                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            _logService.Error("PrefSuite Service: Error inserting items for order {$Order}. Exception {$Exception}", a2pOrders[j].Order, ex.Message);
                        }

                    }

                });

                return _progressValue;

            }
            catch (Exception ex)
            {
                _logService.Error("PrefSuite Service: Unhandled error inserting items. Exception {$Exception}", ex.Message);
                return _progressValue;

            }

        }
    }
}


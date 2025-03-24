// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.DTO;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Data.SqlClient;

namespace a2p.Shared.Infrastructure.Services
{
    public class WriteService : IWriteService
    {
        private readonly ILogService _logService;
        private readonly ISqlRepository _sqlRepository;
        private readonly IPrefSuiteService _prefSuiteService;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;
        private DataCache _dataCache;

        public WriteService(ISqlRepository sqlRepository, ILogService logService, DataCache dataCache, IPrefSuiteService prefSuiteService)
        {
            _sqlRepository = sqlRepository ?? throw new ArgumentNullException(nameof(sqlRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _dataCache = dataCache;
            _prefSuiteService = prefSuiteService;

        }

        public async Task WriteAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progress = progress ?? new Progress<ProgressValue>();
            _progressValue = progressValue ?? new ProgressValue();
            _progressValue.Value = 0;

            try
            {

                List<A2POrder> a2pOrders = _dataCache.GetAllOrders();

                int ordersToProcessCount = a2pOrders.Where(x => x.Import).Count();

                int ItemsToProcessCount = a2pOrders.Where(x => x.Import)
                 .SelectMany(order => order.Items)
                 .Count();
                int materialsToProcessCount = a2pOrders.Where(x => x.Import)
                   .SelectMany(order => order.Materials)
                   .Count();

                _progressValue.ProgressTitle = $"Deleting existing data ...";
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

                _progressValue.MinValue = 0;
                _progressValue.MaxValue = ordersToProcessCount * 60; //'+ materialsToProcessCount;

                int ordersCount = 0;
                foreach (A2POrder a2pOrder in a2pOrders)
                {

                    try
                    {

                        if (a2pOrder.Import == false)
                        {
                            continue;
                        }

                        ordersCount++;

                        _progressValue.ProgressTask1 = $"Deleting existing orders.";
                        _progressValue.ProgressTask2 = string.Empty;
                        _progressValue.ProgressTask3 = string.Empty;
                        _progress?.Report(_progressValue);

                        SqlCommand cmd = new()
                        {
                            CommandText = "[dbo].[Uniwave_a2p_DeleteExistsingData]",
                            CommandType = CommandType.StoredProcedure
                        };

                        _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", a2pOrder.SalesDocumentNumber);
                        _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", a2pOrder.SalesDocumentVersion);

                        int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                        _logService.Debug("Write Item Service: Order:{$Order} Sales Document : {$SalesDocument} marked as deleted in the database.",
                            a2pOrder.Order, a2pOrder.SalesDocumentNumber.ToString() + "/" + a2pOrder.SalesDocumentVersion.ToString());
                        _progressValue.ProgressTitle = $"Importing Orders...";
                        _progressValue.Value += 20; // Increment the progress value - write per Order
                        _progress?.Report(_progressValue);

                        _progressValue = await _prefSuiteService.InsertItemsAsync(_progressValue, _progress);
                        _progressValue.Value += 20; ;  // Increment the progress value - write per Order
                        _progress?.Report(_progressValue);
                        _ = InsertOrderAsync(a2pOrder);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Write Item Service: Unhandled error: deleting items of order: {$Order} from DB. Exception { $Exception}", a2pOrder.Order, ex.Message);
                        InsertWriteError(a2pOrder.Order, ErrorLevel.Error, ErrorCode.DatabaseDelete_Data, $"Order: {a2pOrder.Order}. Error deleting items from DB. {ex.Message}");

                    }

                    finally
                    {
                        _progressValue.Value += 20;  // Increment the progress value - write per Order
                        _progress?.Report(_progressValue);
                    }

                }

            }

            catch (Exception ex)
            {
                _logService.Error("Write Item Service: Unhandled error: deleting items {$Exception}", ex.Message);
            }
        }

        private async Task InsertOrderAsync(A2POrder a2pOrder)
        {
            try
            {

                DateTime dateTime = DateTime.UtcNow;
                int itemCounter = a2pOrder.Items.Count;
                foreach (ItemDTO itemDTO in a2pOrder.Items)
                {

                    try
                    {
                        itemCounter++;

                        _progressValue.ProgressTask2 = $"Importing Items {itemCounter} of {a2pOrder.Items.Count} - Item # {itemDTO.Item}";
                        _progressValue.ProgressTask3 = string.Empty;
                        _progress?.Report(_progressValue);

                        SqlCommand cmd = new()
                        {
                            CommandText = "[dbo].[Uniwave_a2p_InsertItem]",
                            CommandType = CommandType.StoredProcedure
                        };

                        _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", a2pOrder.SalesDocumentNumber);   //required
                        _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", a2pOrder.SalesDocumentVersion);//required 
                        _ = cmd.Parameters.AddWithValue("@SalesDocumentIdPos", itemDTO.SalesDocumentIdPos);                                                   //required
                                                                                                                                                              //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Order", itemDTO.Order); //required
                        _ = cmd.Parameters.AddWithValue("@Worksheet", itemDTO.Worksheet); //require
                        _ = cmd.Parameters.AddWithValue("@Line", itemDTO.Line); //required
                        _ = cmd.Parameters.AddWithValue("@Column", itemDTO.Column); //required
                                                                                    //=====================================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Project", itemDTO.Project ?? (object)DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@Item", itemDTO.Item ?? (object)DBNull.Value);                                                //required
                        _ = cmd.Parameters.AddWithValue("@SortOrder", itemDTO.SortOrder); //required
                        _ = cmd.Parameters.AddWithValue("@Description", itemDTO.Description ?? (object)DBNull.Value);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Quantity", itemDTO.Quantity); //required
                                                                                        //=====================================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Width", itemDTO.Width);
                        _ = cmd.Parameters.AddWithValue("@Height", itemDTO.Height);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Weight", itemDTO.Weight);
                        _ = cmd.Parameters.AddWithValue("@WeightWithoutGlass", itemDTO.WeightWithoutGlass);
                        _ = cmd.Parameters.AddWithValue("@WeightGlass", itemDTO.WeightGlass);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@TotalWeight", itemDTO.TotalWeight);
                        _ = cmd.Parameters.AddWithValue("@TotalWeightWithoutGlass", itemDTO.TotalWeightWithoutGlass);
                        _ = cmd.Parameters.AddWithValue("@TotalWeightGlass", itemDTO.TotalWeightGlass);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Area", itemDTO.Area);
                        _ = cmd.Parameters.AddWithValue("@TotalArea", itemDTO.TotalArea);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Hours", itemDTO.Hours);
                        _ = cmd.Parameters.AddWithValue("@TotalHours", itemDTO.TotalHours);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@MaterialCost", itemDTO.MaterialCost);
                        _ = cmd.Parameters.AddWithValue("@LaborCost", itemDTO.LaborCost);
                        _ = cmd.Parameters.AddWithValue("@Cost", itemDTO.Cost);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@TotalMaterialCost", itemDTO.TotalMaterialCost);
                        _ = cmd.Parameters.AddWithValue("@TotalLaborCost", itemDTO.TotalLaborCost);
                        _ = cmd.Parameters.AddWithValue("@TotalCost", itemDTO.TotalCost);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Price", itemDTO.Price);
                        _ = cmd.Parameters.AddWithValue("@TotalPrice", itemDTO.TotalPrice);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@CurrencyCode", itemDTO.CurrencyCode ?? string.Empty);
                        _ = cmd.Parameters.AddWithValue("@ExchangeRateEUR", itemDTO.ExchangeRateEUR);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@MaterialCostEUR", itemDTO.MaterialCostEUR);
                        _ = cmd.Parameters.AddWithValue("@LaborCostEUR", itemDTO.LaborCostEUR);
                        _ = cmd.Parameters.AddWithValue("@CostEUR", itemDTO.CostEUR);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@TotalMaterialCostEUR", itemDTO.TotalMaterialCostEUR);
                        _ = cmd.Parameters.AddWithValue("@TotalLaborCostEUR", itemDTO.TotalLaborCostEUR);
                        _ = cmd.Parameters.AddWithValue("@TotalCostEUR", itemDTO.TotalCostEUR);
                        //========================================================================================================              
                        _ = cmd.Parameters.AddWithValue("@PriceEUR", itemDTO.PriceEUR);
                        _ = cmd.Parameters.AddWithValue("@TotalPriceEUR", itemDTO.TotalPriceEUR);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@WorksheetType", itemDTO.WorksheetType); //Required
                                                                                                  //=====================================================================================================================
                        _ = cmd.Parameters.AddWithValue("@CreatedUTCDateTime", dateTime); //Required
                        _ = cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", dateTime); //Required

                        _ = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Write Service: Unhandled error: Inserting Sapa v2 itemDTO to DB. Exception { $Exception}", ex.Message);
                        InsertWriteError(a2pOrder.Order, ErrorLevel.Error, ErrorCode.DatabaseWrite_Item, $"Order: {a2pOrder.Order}. Error inserting item {itemDTO.Item} into DB. {ex.Message}");
                    }
                    finally
                    {

                        _progress?.Report(_progressValue);
                    }

                }
                _ = InsertMaterialsAsync(a2pOrder);

            }
            catch (Exception ex)
            {
                _logService.Error("Write Service: Unhandled error: Inserting Sapa v2 itemDTO to DB. Exception { $Exception}", ex.Message);
            }
        }
        private async Task InsertMaterialsAsync(A2POrder a2pOrder)
        {
            try
            {

                DateTime dateTime = DateTime.UtcNow;
                int materialCounter = 0;
                foreach (MaterialDTO materialDTO in a2pOrder.Materials)
                {

                    try
                    {
                        materialCounter++;
                        _progressValue.ProgressTask3 = $"Importing material {materialCounter} of {a2pOrder.Materials.Count} - Reference # {materialDTO.ReferenceBase}";
                        _progress?.Report(_progressValue);

                        SqlCommand cmd = new()
                        {
                            CommandText = "[dbo].[Uniwave_a2p_InsertMaterial]",
                            CommandType = CommandType.StoredProcedure
                        };

                        _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", a2pOrder.SalesDocumentNumber); //required
                        _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", a2pOrder.SalesDocumentVersion); //required
                                                                                                                 //=====================================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Order", materialDTO.Order); //required
                        _ = cmd.Parameters.AddWithValue("@Worksheet", materialDTO.Worksheet); //required
                        _ = cmd.Parameters.AddWithValue("@Line", materialDTO.Line); //required
                        _ = cmd.Parameters.AddWithValue("@Column", materialDTO.Column); //required 
                                                                                        //=====================================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Item", materialDTO.Item ?? (object)DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@SortOrder", materialDTO.SortOrder);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.ReferenceBase);
                        _ = cmd.Parameters.AddWithValue("@Reference", materialDTO.Reference);
                        _ = cmd.Parameters.AddWithValue("@Description", materialDTO.Description ?? (object)DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color);
                        _ = cmd.Parameters.AddWithValue("@ColorDescription", materialDTO.ColorDescription ?? (object)DBNull.Value);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Width", materialDTO.Width);
                        _ = cmd.Parameters.AddWithValue("@Height", materialDTO.Height);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Quantity", materialDTO.Quantity);
                        _ = cmd.Parameters.AddWithValue("@PackageQuantity", materialDTO.PackageQuantity);
                        _ = cmd.Parameters.AddWithValue("@TotalQuantity", materialDTO.TotalQuantity);
                        _ = cmd.Parameters.AddWithValue("@RequiredQuantity", materialDTO.RequiredQuantity);
                        _ = cmd.Parameters.AddWithValue("@LeftOverQuantity", materialDTO.LeftOverQuantity);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Weight", materialDTO.Weight);
                        _ = cmd.Parameters.AddWithValue("@TotalWeight", materialDTO.TotalWeight);
                        _ = cmd.Parameters.AddWithValue("@RequiredWeight", materialDTO.RequiredWeight);
                        _ = cmd.Parameters.AddWithValue("@LeftOverWeight", materialDTO.LeftOverWeight);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Area", materialDTO.Area);
                        _ = cmd.Parameters.AddWithValue("@TotalArea", materialDTO.TotalArea);
                        _ = cmd.Parameters.AddWithValue("@RequiredArea", materialDTO.RequiredArea);
                        _ = cmd.Parameters.AddWithValue("@LeftOverArea", materialDTO.LeftOverArea);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Waste", materialDTO.Waste);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Price", materialDTO.Price);
                        _ = cmd.Parameters.AddWithValue("@TotalPrice", materialDTO.TotalPrice);
                        _ = cmd.Parameters.AddWithValue("@RequiredPrice", materialDTO.RequiredPrice);
                        _ = cmd.Parameters.AddWithValue("@LeftOverPrice", materialDTO.LeftOverPrice);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@SquareMeterPrice", materialDTO.SquareMeterPrice);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Pallet", materialDTO.Pallet ?? (object)DBNull.Value);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@MaterialType", materialDTO.MaterialType);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@WorksheetType", materialDTO.WorksheetType);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@CustomField1", materialDTO.CustomField1 ?? (object)DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@CustomField2", materialDTO.CustomField2 ?? (object)DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@CustomField3", materialDTO.CustomField3 ?? (object)DBNull.Value);
                        //========================================================================================================;
                        _ = cmd.Parameters.AddWithValue("@CustomField4", materialDTO.CustomField4 ?? (object)DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@CustomField5", materialDTO.CustomField5 ?? (object)DBNull.Value);
                        //========================================================================================================              
                        _ = cmd.Parameters.AddWithValue("@SourceReference", materialDTO.SourceReference ?? (object)DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@SourceDescription", materialDTO.SourceDescription ?? (object)DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@SourceColor", materialDTO.SourceColor ?? (object)DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@SourceColorDescription", materialDTO.SourceColorDescription ?? (object)DBNull.Value);
                        //========================================================================================================              
                        _ = cmd.Parameters.AddWithValue("@CreatedUTCDateTime", dateTime);
                        _ = cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", dateTime);

                        int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                        _ = await InsertPrefSuiteMaterialAsync(materialDTO);

                        _logService.Verbose("Write Service: Inserted material. Order: {$Order} Inserting material {$Material} into DB.", a2pOrder.Order, materialDTO.Reference);
                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Write Service: Unhandled Error inserting material. Order: {$Order} Inserting material {$Material} into DB. Error Message {$Exception}", a2pOrder.Order, materialDTO.Reference, ex.Message);
                        InsertWriteError(a2pOrder.Order, ErrorLevel.Error, ErrorCode.DatabaseWrite_Material, $"Order: {a2pOrder.Order}. Error inserting material {materialDTO.Reference} into DB. {ex.Message}");
                        continue;
                    }

                    finally
                    {
                        //   _progressValue.Value++;// Increment the progress value - Read per Material
                        _progress?.Report(_progressValue);

                    }

                }
                _ = await InsertMaterialNeedsAsync(a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);

                _logService.Debug("Write Service: All Materials Inserted: {$Order} Inserting material {$Material} into DB.", a2pOrder.Order);
            }
            catch (Exception ex)
            {
                _logService.Debug(ex, "Unhandled error: inserting Sapa v2 Material to DB");

            }

        }

        private async Task<int> InsertMaterialNeedsAsync(int salesDocumentNumber, int salesDocumentVersion)
        {
            const int maxRetries = 3;
            int attempt = 0;

            while (attempt < maxRetries)
            {
                try
                {
                    _progressValue.ProgressTask2 = $"Inserting Material Needs for Sales Document {salesDocumentNumber}/{salesDocumentVersion}";
                    _progressValue.ProgressTask3 = string.Empty;
                    _progress?.Report(_progressValue);

                    SqlCommand cmd = new()
                    {
                        CommandText = "[dbo].[Uniwave_a2p_MaterialNeeds]",
                        CommandType = CommandType.StoredProcedure
                    };

                    _ = cmd.Parameters.AddWithValue("@Number", salesDocumentNumber);
                    _ = cmd.Parameters.AddWithValue("@Version", salesDocumentVersion);

                    int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                    return result;
                }
                catch (SqlException ex) when (ex.Number == 1205) // Deadlock
                {
                    attempt++;
                    _logService.Warning(ex, $"Deadlock detected on attempt {attempt}. Retrying...");

                    if (attempt >= maxRetries)
                    {
                        _logService.Error(ex, "Max retry attempts reached. Deadlock could not be resolved.");
                        return -1;
                    }

                    await Task.Delay(500 * attempt); // Exponential backoff
                }
                catch (Exception ex)
                {
                    _logService.Error(ex, "Unhandled error: inserting Material Needs to DB");
                    return -1;
                }
            }

            return -1;
        }

        private async Task<int> InsertPrefSuiteMaterialAsync(MaterialDTO materialDTO)
        {
            try
            {
                _ = await CreatePrefSuiteColorAsync(materialDTO);

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefMaterial]",
                    CommandType = CommandType.StoredProcedure
                };
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", materialDTO.Reference);
                _ = cmd.Parameters.AddWithValue("@Reference", materialDTO.Reference);
                _ = cmd.Parameters.AddWithValue("@Description", materialDTO.Description ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color);
                //=======================================================================================
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", materialDTO.PackageQuantity);
                _ = cmd.Parameters.AddWithValue("@Weight", materialDTO.Weight);
                _ = cmd.Parameters.AddWithValue("@MaterialType", materialDTO.MaterialType);

                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                if (result > 0)
                {
                    _logService.Debug("Write Service: PrefSuite {$Reference} Material created successfully", materialDTO.Reference);

                }
                return result;
            }
            catch (Exception ex)

            {

                _logService.Error(ex, "Unhandled error: inserting Sapa v2 Material to DB");
                return -1;

            }

        }
        private async Task<int> CreatePrefSuiteColorAsync(MaterialDTO materialDTO)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefColor]",
                    CommandType = CommandType.StoredProcedure
                };
                _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color);
                _ = cmd.Parameters.AddWithValue("@ColorDescription", materialDTO.ColorDescription ?? (object)DBNull.Value);

                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                if (result > 0)
                {
                    _logService.Debug("Write Service: PrefSuite {$Color} Material insert successfully", materialDTO.Color);

                }
                return result;

            }
            catch (Exception ex)

            {

                _logService.Error(ex, "Unhandled error: inserting Sapa v2 Material to DB");
                return -1;

            }

        }

        private void InsertWriteError(string order, ErrorLevel errorLevel, ErrorCode errorCode, string message)
        {

            A2POrderError a2pOrderError = new()
            {
                Order = order,
                Level = errorLevel,
                Code = errorCode,
                Message = message
            };
            _dataCache.UpdateOrderInCache(order, updatedOrder =>
            {
                updatedOrder.WriteErrors.Add(a2pOrderError);
            });

        }

        public Task InsertDataAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null) => throw new NotImplementedException();
    }
}

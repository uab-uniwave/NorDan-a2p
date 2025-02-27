// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Data;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.DTO;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Data.SqlClient;

namespace a2p.Shared.Infrastructure.Services
{
    public class WriteMaterialService : IWriteMaterialService
    {
        private readonly ILogService _logService;
        private readonly ISqlRepository _sqlRepository;
        private readonly IPrefSuiteService _prefSuiteService;
        private readonly DataCache _dataCache;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public WriteMaterialService(ISqlRepository sqlRepository, ILogService logService, IPrefSuiteService prefSuiteService, DataCache dataCache)
        {
            _sqlRepository = sqlRepository;
            _logService = logService;
            _prefSuiteService = prefSuiteService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _dataCache = dataCache;
        }

        public async Task InsertListAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            try
            {
                List<A2POrder> a2pOrders = _dataCache.GetAllOrders();

                _progressValue = progressValue;
                _progress = progress ?? new Progress<ProgressValue>();

                foreach (A2POrder a2pOrder in a2pOrders)
                {
                    _progressValue.ProgressTask1 = $"Importing Order# {a2pOrder.Order}. Order {a2pOrder.Order}  of {a2pOrders.Count}";
                    _progress?.Report(_progressValue);
                    if (a2pOrder.Materials == null)
                    {
                        _logService.Error("WS:Error inserting materials to DB. Materials are null for order: {$Order}", a2pOrder.Order);

                        continue;

                    }

                    DateTime dateTime = DateTime.UtcNow;
                    int updatedDBRecords = 0;
                    int materialCount = 0;
                    foreach (MaterialDTO materialDTO in a2pOrder.Materials)
                    {

                        {
                            _progressValue.ProgressTask3 = $"Importing Material# {materialCount + 1} of {a2pOrder.Materials.Count}";
                            _progress?.Report(_progressValue);

                            SqlCommand cmd = new()
                            {
                                CommandText = "[dbo].[Uniwave_a2p_InsertMaterial]",
                                CommandType = CommandType.StoredProcedure
                            };
                            _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", a2pOrder.SalesDocumentNumber); //required
                            _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", a2pOrder.SalesDocumentVersion); //required
                                                                                                                     //========================================================================================================
                            _ = cmd.Parameters.AddWithValue("@Order", materialDTO.Order); //required
                            _ = cmd.Parameters.AddWithValue("@Worksheet", materialDTO.Worksheet); //required
                            _ = cmd.Parameters.AddWithValue("@Line", materialDTO.Line); //required
                            _ = cmd.Parameters.AddWithValue("@Column", materialDTO.Column); //required 
                                                                                            //========================================================================================================
                            _ = cmd.Parameters.AddWithValue("@Item", materialDTO.Item ?? (object)DBNull.Value);
                            _ = cmd.Parameters.AddWithValue("@SortOrder", materialDTO.SortOrder);
                            //========================================================================================================
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

                            if (result > 0)
                            {
                                _ = await InsertPrefSuiteMaterialAsync(materialDTO);
                                updatedDBRecords = +result;

                            }

                            materialCount++;

                        }

                    }

                    _ = await InsertMaterialNeedsAsync(a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);

                }

                {

                }

            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Write Service: Unhandled error: inserting Sapa v2 Material to DB");
            }
        }

        private async Task<int> InsertMaterialNeedsAsync(int salesDocumentNumber, int salesDocumentVersion)
        {
            try
            {

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
            catch (Exception ex)
            {

                _logService.Error(ex, "Unhandled error: inserting Sapa v2 Material to DB");
                return -1;

            }
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
                _ = cmd.Parameters.AddWithValue("@Reference", materialDTO.Reference);
                _ = cmd.Parameters.AddWithValue("@Description", materialDTO.Description ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color);
                //=======================================================================================
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", materialDTO.PackageQuantity);
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

        public async Task DeleteAsync(string order)
        {
            try
            {
                if (order == null)
                {

                    _logService.Error("WS:Error deleting (Update DeletedUTCDateTime) materials from DB. Order is null");

                    return;
                }

                DateTime dateTime = DateTime.UtcNow;

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_DeleteMaterials]",
                    CommandType = CommandType.StoredProcedure
                };
                _ = cmd.Parameters.AddWithValue("@Order", order);
                _ = cmd.Parameters.AddWithValue("@DeletedUTCDateTime", dateTime);
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Debug("WS: Delete Materials. {$result} material records of order: {$Order} deleted from db.", result, order);
                }
                else
                {
                    _logService.Debug("WS: Delete Materials. No material records of order: {$Order} deleted from db.", order);
                }

            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "WS: Unhandled error: deleting Sapa v2 order: {$Order} materials from DB", order);

            }
        }


    }

}

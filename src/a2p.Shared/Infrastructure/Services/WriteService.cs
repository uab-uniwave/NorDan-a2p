using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Services;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Shared.Infrastructure.Services
{
    public class WriteService : IWriteService
    {
        private readonly ILogService _logService;
        private readonly ISqlRepoitory _sqlRepository;

        public WriteService(ISqlRepoitory sqlRepository, ILogService logService)
        {
            _sqlRepository = sqlRepository;
            _logService = logService;
        }


        public async Task<int> InsertItemAsync(ItemDTO itemDTO, int salesDocumentNumber, int salesDocumentVersion, DateTime dateTime)
        {
            try
            {
                if (itemDTO == null)
                {
                    _logService.Debug("Error inserting itemDTO to DB: itemDTO is null");
                    return -1;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertItem]",
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@SalesDocumentNumber", salesDocumentNumber);                       //required
                cmd.Parameters.AddWithValue("@SalesDocumentVersion", salesDocumentVersion);                     //required
                //========================================================================================================
                cmd.Parameters.AddWithValue("@Order", itemDTO.Order);                                              //required
                cmd.Parameters.AddWithValue("@Worksheet", itemDTO.Worksheet);                                      //required
                cmd.Parameters.AddWithValue("@Line", itemDTO.Line);                                                //required
                cmd.Parameters.AddWithValue("@Column", itemDTO.Column);                                            //required
                //========================================================================================================
                cmd.Parameters.AddWithValue("@Project", itemDTO.Project ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Item", itemDTO.Item);                                                //required
                cmd.Parameters.AddWithValue("@SortOrder", itemDTO.SortOrder);                                      //required
                cmd.Parameters.AddWithValue("@Description", itemDTO.Description ?? (object)DBNull.Value);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@Quantity", itemDTO.Quantity);                                        //required
                //========================================================================================================
                cmd.Parameters.AddWithValue("@Width", itemDTO.Width ?? 0);
                cmd.Parameters.AddWithValue("@Height", itemDTO.Height ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@Weight", itemDTO.Weight ?? (object)DBNull.Value ?? 0);
                cmd.Parameters.AddWithValue("@WeightWithoutGlass", itemDTO.WeightWithoutGlass ?? 0);
                cmd.Parameters.AddWithValue("@WeightGlass", itemDTO.WeightGlass ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@TotalWeight", itemDTO.TotalWeight ?? 0);
                cmd.Parameters.AddWithValue("@TotalWeightWithoutGlass", itemDTO.TotalWeightWithoutGlass ?? 0);
                cmd.Parameters.AddWithValue("@TotalWeightGlass", itemDTO.TotalWeightGlass ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@Area", itemDTO.Area ?? 0);
                cmd.Parameters.AddWithValue("@TotalArea", itemDTO.TotalArea ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@Hours", itemDTO.Hours ?? 0);
                cmd.Parameters.AddWithValue("@TotalHours", itemDTO.Hours ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@MaterialCost", itemDTO.MaterialCost ?? 0);
                cmd.Parameters.AddWithValue("@LaborCost", itemDTO.LaborCost ?? 0);
                cmd.Parameters.AddWithValue("@Cost", itemDTO.Cost ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@TotalMaterialCost", itemDTO.MaterialCost ?? 0);
                cmd.Parameters.AddWithValue("@TotalLaborCost", itemDTO.LaborCost ?? 0);
                cmd.Parameters.AddWithValue("@TotalCost", itemDTO.Cost ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@Price", itemDTO.Price ?? 0);
                cmd.Parameters.AddWithValue("@TotalPrice", itemDTO.TotalPrice ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@CurrencyCode", itemDTO.CurrencyCode);
                cmd.Parameters.AddWithValue("@ExchangeRateEUR", itemDTO.ExchangeRateEUR);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@MaterialCostEUR", itemDTO.MaterialCostEUR ?? 0);
                cmd.Parameters.AddWithValue("@LaborCostEUR", itemDTO.LaborCostEUR ?? 0);
                cmd.Parameters.AddWithValue("@CostEUR", itemDTO.TotalCostEUR ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@TotalMaterialCostEUR", itemDTO.MaterialCostEUR ?? 0);
                cmd.Parameters.AddWithValue("@TotalLaborCostEUR", itemDTO.LaborCostEUR ?? 0);
                cmd.Parameters.AddWithValue("@TotalCostEUR", itemDTO.TotalCostEUR ?? 0);
                //========================================================================================================              
                cmd.Parameters.AddWithValue("@PriceEUR", itemDTO.PriceEUR ?? 0);
                cmd.Parameters.AddWithValue("@TotalPriceEUR", itemDTO.TotalPriceEUR ?? 0);
                //========================================================================================================
                cmd.Parameters.AddWithValue("@WorksheetType", itemDTO.WorksheetType);                              //Required
                //========================================================================================================
                cmd.Parameters.AddWithValue("@CreatedUTCDateTime", dateTime);                                   //Required
                cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", dateTime);                                  //Required


                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                return result;
            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: inserting Sapa v2 itemDTO to DB");
                return -1;
            }
        }
        public async Task<int> InsertMaterialAsync(MaterialDTO materialDTO, int salesDocumentNumber, int salesDocumentVersion, DateTime dateTime)
        {
            try
            {
                if (materialDTO == null)
                {

                    _logService.Debug("Error Inserting Sapa v2 Material to DB, material is null");
                    return -1;

                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertMaterial]",
                    CommandType = CommandType.StoredProcedure
                };
                _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", salesDocumentNumber);               //required
                _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", salesDocumentVersion);             //required
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Order", materialDTO.Order);                               //required
                _ = cmd.Parameters.AddWithValue("@Worksheet", materialDTO.Worksheet);                       //required
                _ = cmd.Parameters.AddWithValue("@Line", materialDTO.Line);                                 //required
                _ = cmd.Parameters.AddWithValue("@Column", materialDTO.Column);                             //required 
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Item", materialDTO.Item ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@SortOrder", materialDTO.SortOrder ?? -1);
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
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", materialDTO.PackageQuantity ?? 1);
                _ = cmd.Parameters.AddWithValue("@TotalQuantity", materialDTO.TotalQuantity ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@RequiredQuantity", materialDTO.RequiredQuantity);
                _ = cmd.Parameters.AddWithValue("@LeftOverQuantity", materialDTO.LeftOverQuantity ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Weight", materialDTO.Weight ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@TotalWeight", materialDTO.TotalWeight ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@RequiredWeight", materialDTO.RequiredWeight ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@LeftOverWeight", materialDTO.LeftOverWeight ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Area", materialDTO.Area ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@TotalArea", materialDTO.TotalArea ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@RequiredArea", materialDTO.RequiredArea ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@LeftOverArea", materialDTO.LeftOverArea ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Waste", materialDTO.Waste ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Price", materialDTO.Price ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@TotalPrice", materialDTO.TotalPrice ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@RequiredPrice", materialDTO.RequiredPrice ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@LeftOverPrice", materialDTO.LeftOverPrice ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@SquareMeterPrice", materialDTO.SquareMeterPrice ?? (object)DBNull.Value);
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
                return result;

            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: inserting Sapa v2 glass to DB");
                return -1;
            }
        }
        public async Task<int> DeleteMaterialsAsync(string order)
        {
            try
            {
                if (order == null)
                {

                    _logService.Error("WS:Error deleting (Update DeletedUTCDateTime) materials from DB. Order is null");
                    return -1;

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


                _logService.Debug("WS: Delete Materials. {$result} material records of order: {$Order} deleted from db.", result, order);

                return result;

            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, "WS: Unhandled error: deleting Sapa v2 order: {$Order} materials from DB", order);
                return -1;
            }
        }
        public async Task<int> DeleteItemsAsync(string order)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(order))
                {
                    _logService.Error("WS: Error deleting (updating DeletedUTCDateTime) items from DB. Order is null or empty.");
                    return -1;
                }

                DateTime dateTime = DateTime.UtcNow;

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_DeleteItems]",
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@Order", order);
                cmd.Parameters.AddWithValue("@DeletedUTCDateTime", dateTime);

                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                _logService.Debug($"WS: Delete Items. {result} itemDTO records of order: {order} marked as deleted in the database.");

                return result;
            }
            catch (Exception ex)
            {
                _logService.Error(ex.Message, $"WS: Unhandled error: deleting items of order: {order} from DB.");
                return -1;
            }
        }
    }
}
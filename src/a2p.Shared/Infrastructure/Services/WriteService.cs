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

                _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", salesDocumentNumber);                       //required
                _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", salesDocumentVersion);                     //required
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Order", itemDTO.Order);                                             //required
                _ = cmd.Parameters.AddWithValue("@Worksheet", itemDTO.Worksheet);                                      //require
                _ = cmd.Parameters.AddWithValue("@Line", itemDTO.Line);                                                //required
                _ = cmd.Parameters.AddWithValue("@Column", itemDTO.Column);                                            //required
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Project", itemDTO.Project ?? (object)DBNull.Value);
                _ = cmd.Parameters.AddWithValue("@Item", itemDTO.Item ?? (object)DBNull.Value);                                                //required
                _ = cmd.Parameters.AddWithValue("@SortOrder", itemDTO.SortOrder);                                      //required
                _ = cmd.Parameters.AddWithValue("@Description", itemDTO.Description ?? (object)DBNull.Value);
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@Quantity", itemDTO.Quantity);                                        //required
                //========================================================================================================
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
                _ = cmd.Parameters.AddWithValue("@WorksheetType", itemDTO.WorksheetType);                           //Required
                //========================================================================================================
                _ = cmd.Parameters.AddWithValue("@CreatedUTCDateTime", dateTime);                                   //Required
                _ = cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", dateTime);                                  //Required


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
                return result;

            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: inserting Sapa v2 Material to DB");
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

                _ = cmd.Parameters.AddWithValue("@Order", order);
                _ = cmd.Parameters.AddWithValue("@DeletedUTCDateTime", dateTime);

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
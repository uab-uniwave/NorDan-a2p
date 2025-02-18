using a2p.Shared.Core.DTO;
using a2p.Shared.Infrastructure.Interfaces;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Shared.Infrastructure.Services
{
    public class WriteItemService : IWriteItemService
    {
        private readonly ILogService _logService;
        private readonly ISqlRepository _sqlRepository;
        private readonly IPrefSuiteIntegrationService _prefSuiteIntegrationService;

        public WriteItemService(ISqlRepository sqlRepository, ILogService logService, IPrefSuiteIntegrationService prefSuiteIntegrationService)
        {
            _sqlRepository = sqlRepository ?? throw new ArgumentNullException(nameof(sqlRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _prefSuiteIntegrationService = prefSuiteIntegrationService ?? throw new ArgumentNullException(nameof(prefSuiteIntegrationService));
        }

        public Task<int> DeleteAsync(string order)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(ItemDTO itemDTO, int salesDocumentNumber, int salesDocumentVersion, DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public async Task<int> InsertItemAsync(ItemDTO itemDTO, int salesDocumentNumber, int salesDocumentVersion, DateTime dateTime)
        {
            try
            {
                if (itemDTO == null)
                {
                    _logService.Debug("Write Service: Error inserting itemDTO to DB: itemDTO is null");
                    return -1;
                }

                Task<string> salesDocIdPos = _prefSuiteIntegrationService.AddItem(itemDTO, salesDocumentNumber, salesDocumentVersion);


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertItem]",
                    CommandType = CommandType.StoredProcedure
                };

                _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", salesDocumentNumber);                       //required
                _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", salesDocumentVersion);                     //required
                _ = cmd.Parameters.AddWithValue("@SalesDocIdPos", salesDocIdPos);                                                   //required
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







                //salesDoc.ConnectionString = "Provider=SQLOLEDB.1;Persist Security Info=False;Data Source=COSMOS;Initial Catalog=Reproduction.2018.2.Fauga;UID=sa;PWD=1234567;Trusted_Connection=Yes";//Util.myConnection.OleConnectionString;





                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());


                return result;
            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Write Service: Unhandled error: inserting Sapa v2 itemDTO to DB");
                return -1;
            }
        }
    }
}
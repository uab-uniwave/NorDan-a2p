using System.Data;

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

using DocumentFormat.OpenXml.Drawing.Charts;

using Microsoft.Data.SqlClient;

namespace a2p.Shared.Infrastructure.Services
{
    public class WriteItemService : IWriteItemService
    {
        private readonly ILogService _logService;
        private readonly ISqlRepository _sqlRepository;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;
        private DataCache _dataCache;

        public WriteItemService(ISqlRepository sqlRepository, ILogService logService, DataCache dataCache)
        {
            _sqlRepository = sqlRepository ?? throw new ArgumentNullException(nameof(sqlRepository));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));

            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
            _dataCache = dataCache;
        }

        public async Task DeleteAsync(string order)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(order))
                {
                    _logService.Error("WS: Error deleting (updating DeletedUTCDateTime) items from DB. Order is null or empty.");
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

                if (result > 0)
                    _logService.Debug($"Write Item Service: Delete Items. {result} item records of order: {order} marked as deleted in the database.");
                else
                    _logService.Warning($"Write Item Service: No items deleted. {result} item records of order: {order}.");

            }
            catch (Exception ex)
            {
                _logService.Error("Write Item Service: Unhandled error: deleting items of order: {$Order} from DB. Exception { $Exception}", order, ex.Message);
                var a2pOrder = _dataCache.GetOrder(order);
                if (a2pOrder != null)
                {
                    A2POrderError writeError = new A2POrderError
                    {
                        Order = order,
                        Level = ErrorLevel.Error,
                        Code = ErrorCode.DatabaseWrite_Item,
                        Message = $"Write Item Service: Unhandled error: deleting items of order: {a2pOrder.Order} from DB. Exception {ex.Message}"
                    };

                    a2pOrder.WriteErrors.Add(writeError);

                    _dataCache.UpdateOrderInCache(order, updatedOrder =>
                    {
                        updatedOrder.WriteErrors.Add(writeError);
                    });
                }
            }
               
            
        }

        public async Task InsertListAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            var a2pOrders = _dataCache.GetAllOrders();

            try
            {



                _progressValue = progressValue;
                _progress = progress ?? new Progress<ProgressValue>();

                foreach (A2POrder a2pOrder in a2pOrders)
                {



                    DateTime dateTime = DateTime.UtcNow;

                    int updatedDBRecords = 0;
                    int itemCount = 0;

                    foreach (ItemDTO itemDTO in a2pOrder.Items)
                    {
                        itemCount++;

                        _progressValue.ProgressTask2 = $"Importing Items into DB. {itemCount} of {a2pOrder.Items.Count}";
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
                                                                                    //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Project", itemDTO.Project ?? (object) DBNull.Value);
                        _ = cmd.Parameters.AddWithValue("@Item", itemDTO.Item ?? (object) DBNull.Value);                                                //required
                        _ = cmd.Parameters.AddWithValue("@SortOrder", itemDTO.SortOrder - 1); //required
                        _ = cmd.Parameters.AddWithValue("@Description", itemDTO.Description ?? (object) DBNull.Value);
                        //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@Quantity", itemDTO.Quantity); //required
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
                        _ = cmd.Parameters.AddWithValue("@WorksheetType", itemDTO.WorksheetType); //Required
                                                                                                  //========================================================================================================
                        _ = cmd.Parameters.AddWithValue("@CreatedUTCDateTime", dateTime); //Required
                        _ = cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", dateTime); //Required

                        updatedDBRecords = +await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                   
                    }
                    if (updatedDBRecords > 0)
                    {
                        _logService.Debug($"Write Service: {updatedDBRecords} item records of order: {a2pOrder.Order} inserted into the database.");
                    }
                    else
                    {
                        _logService.Warning($"Write Service: No item records of order: {a2pOrder.Order} inserted into the database.");
                        A2POrderError writeError = new A2POrderError
                        {
                            Order = a2pOrder.Order,
                            Level = ErrorLevel.Warning,
                            Code = ErrorCode.DatabaseWrite_Item,
                            Message = $"Order: {a2pOrder.Order}. No Items inserted into DB."
                        };


                        a2pOrder.WriteErrors.Add(writeError);
                        _dataCache.UpdateOrderInCache(a2pOrder.Order, updatedOrder =>
                        {
                            updatedOrder.WriteErrors.Add(writeError);
                        });

                    }

                }

        
            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Write Service: Unhandled error: Inserting Sapa v2 itemDTO to DB");
            }
        }

    }
}

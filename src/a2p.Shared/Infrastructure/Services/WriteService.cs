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
        //public async Task<string?>? GetColorAsync(string color)
        //{
        // try
        // {
        //  string sql = "SELECT Color FROM Colors WHERE Color = '" + color + "'";

        //  string? result = await _sqlService.ExecuteScalarAsync(sql);
        //  return result;//TODO: fix this
        // }
        // catch (Exception ex)
        // {
        //  _logService.Debug(ex.Message, "Unhandled error: getting color from DB");
        //  return null;
        // }
        //}

        public async Task<int> InsertItemAsync(ItemDTO item)
        {
            try
            {
                if (item == null)
                {
                    _logService.Debug("Error Inserting Sapa v2 Item to DB, item is null");
                    throw new ArgumentNullException(nameof(item));
                }

                SqlCommand cmd = new()
                {
                    CommandText = "Uniwave_SAPAInsertPositions",
                    CommandType = CommandType.StoredProcedure
                };

                _ = cmd.Parameters.AddWithValue("@Order", item.Order);
                _ = cmd.Parameters.AddWithValue("@Phase", "no phase "); // TODO: Add phase
                _ = cmd.Parameters.AddWithValue("@Item", item.Item);
                _ = cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                _ = cmd.Parameters.AddWithValue("@Width", item.Width);
                _ = cmd.Parameters.AddWithValue("@Height", item.Height);
                _ = cmd.Parameters.AddWithValue("@Weight", item.Weight);
                _ = cmd.Parameters.AddWithValue("@WeightWithoutGlass", item.WeightWithoutGlass);
                _ = cmd.Parameters.AddWithValue("@MaterialCost", item.MaterialCost);
                _ = cmd.Parameters.AddWithValue("@LaborCost", item.LaborCost);
                _ = cmd.Parameters.AddWithValue("@Price", item.Price);
                _ = cmd.Parameters.AddWithValue("@TotalPrice", item.TotalPrice);
                _ = cmd.Parameters.AddWithValue("@PriceEUR", item.PriceEUR);
                _ = cmd.Parameters.AddWithValue("@TotalPriceEUR", item.TotalPriceEUR);
                _ = cmd.Parameters.AddWithValue("@MaterialCostEUR", item.MaterialCostEUR);
                _ = cmd.Parameters.AddWithValue("@LaborCostEUR", item.LaborCostEUR);
                _ = cmd.Parameters.AddWithValue("@Order", "");
                _ = cmd.Parameters.AddWithValue("@Version", "");
                _ = cmd.Parameters.AddWithValue("@SortOrder", item.SortOrder);
                _ = cmd.Parameters.AddWithValue("@Modified", DateTime.UtcNow);

                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result;
            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: inserting Sapa v2 item to DB");
                return -1;
            }
        }

        public async Task<int> InsertMaterialAsync(MaterialDTO materialDTO)
        {
            try
            {
                if (materialDTO == null)
                {
                    _logService.Debug("Error Inserting Sapa v2 Material to DB, material is null");
                    throw new ArgumentNullException(nameof(materialDTO));
                }

                SqlCommand cmd = new()
                {
                    CommandText = "Uniwave_a2pInsertMNRecord",
                    CommandType = CommandType.StoredProcedure
                };
                _ = cmd.Parameters.AddWithValue("@SalesDocumentNumber", DateTime.UtcNow);
                _ = cmd.Parameters.AddWithValue("@SalesDocumentVersion", DateTime.UtcNow);
                _ = cmd.Parameters.AddWithValue("@Order", materialDTO.Order);

                _ = cmd.Parameters.AddWithValue("@Reference", materialDTO.Reference);
                _ = cmd.Parameters.AddWithValue("@Description", materialDTO.Description);
                _ = cmd.Parameters.AddWithValue("@Color", materialDTO.Color);
                _ = cmd.Parameters.AddWithValue("@ColorDescription", materialDTO.ColorDescription);
                _ = cmd.Parameters.AddWithValue("@Quantity", materialDTO.Quantity);
                _ = cmd.Parameters.AddWithValue("@PackageUnit", materialDTO.PackageUnit);
                _ = cmd.Parameters.AddWithValue("@Price", materialDTO.Price);
                _ = cmd.Parameters.AddWithValue("@TotalPrice", materialDTO.TotalPrice);
                _ = cmd.Parameters.AddWithValue("@QuantityOrdered", materialDTO.QuantityOrdered);
                _ = cmd.Parameters.AddWithValue("@QuantityRequired", materialDTO.QuantityRequired);
                _ = cmd.Parameters.AddWithValue("@Waste", materialDTO.Waste);
                _ = cmd.Parameters.AddWithValue("@Area", materialDTO.Area);
                _ = cmd.Parameters.AddWithValue("@Weight", materialDTO.Weight);
                _ = cmd.Parameters.AddWithValue("@CustomField1", materialDTO.CustomField1);
                _ = cmd.Parameters.AddWithValue("@CustomField2", materialDTO.CustomField2);
                _ = cmd.Parameters.AddWithValue("@CustomField3", materialDTO.CustomField3);
                _ = cmd.Parameters.AddWithValue("@SourceWorksheet", materialDTO.Worksheet);
                _ = cmd.Parameters.AddWithValue("@SourceWorkSheetLine", materialDTO.Line);
                _ = cmd.Parameters.AddWithValue("@SourceWorkSheetColumn", materialDTO.Column);
                _ = cmd.Parameters.AddWithValue("@SourceReference", materialDTO.SourceReference);
                _ = cmd.Parameters.AddWithValue("@SourceDescription", materialDTO.SourceDescription);
                _ = cmd.Parameters.AddWithValue("@SourceColor", materialDTO.SourceColor);
                _ = cmd.Parameters.AddWithValue("@SourceType", materialDTO.Type.ToString());
                _ = cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", DateTime.UtcNow);
                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                return result;
            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: inserting Sapa v2 item to DB");
                return -1;
            }
        }
        public async Task<int> InsertGlassAsync(GlassDTO glassDTO)
        {
            try
            {
                if (glassDTO == null)
                {
                    _logService.Debug("Error Inserting Sapa v2 Glass to DB, glass is null");
                    throw new ArgumentNullException(nameof(glassDTO));
                }

                SqlCommand cmd = new()
                {
                    CommandText = "Uniwave_a2pInsertMNRecordGlass",
                    CommandType = CommandType.StoredProcedure
                };


                _ = cmd.Parameters.AddWithValue("@Order", glassDTO.Order);
                _ = cmd.Parameters.AddWithValue("@Item", glassDTO.Item);
                _ = cmd.Parameters.AddWithValue("@SortOrder", glassDTO.SortOrder);
                _ = cmd.Parameters.AddWithValue("@Reference", glassDTO.Reference);
                _ = cmd.Parameters.AddWithValue("@Description", glassDTO.Description);





                _ = cmd.Parameters.AddWithValue("@Quantity", glassDTO.Quantity);
                _ = cmd.Parameters.AddWithValue("@Width", glassDTO.Width);
                _ = cmd.Parameters.AddWithValue("@Height", glassDTO.Height);
                _ = cmd.Parameters.AddWithValue("@Weight", glassDTO.Weight);
                _ = cmd.Parameters.AddWithValue("@Area", glassDTO.Area);
                _ = cmd.Parameters.AddWithValue("@Price", glassDTO.Price);
                _ = cmd.Parameters.AddWithValue("@TotalPrice", glassDTO.TotalPrice);
                _ = cmd.Parameters.AddWithValue("@SquareMeterPrice", glassDTO.SquareMeterPrice);
                _ = cmd.Parameters.AddWithValue("@TotalWeight", glassDTO.TotalArea);
                _ = cmd.Parameters.AddWithValue("@TotalArea", glassDTO.TotalArea);
                _ = cmd.Parameters.AddWithValue("@AreaUsed", glassDTO.AreaUsed);
                _ = cmd.Parameters.AddWithValue("@Ordered", glassDTO.Ordered);
                _ = cmd.Parameters.AddWithValue("@Waste", glassDTO.Waste);
                _ = cmd.Parameters.AddWithValue("@Pallet", glassDTO.Pallet);
                _ = cmd.Parameters.AddWithValue("@SourceWorksheet", glassDTO.Worksheet);
                _ = cmd.Parameters.AddWithValue("@SourceWorksheetLine", glassDTO.Line);
                _ = cmd.Parameters.AddWithValue("@SourceWorksheetColumn", glassDTO.Column);
                _ = cmd.Parameters.AddWithValue("@Modified", DateTime.UtcNow);
                _ = cmd.Parameters.AddWithValue("@SourceReference", glassDTO.SourceReference);
                _ = cmd.Parameters.AddWithValue("@SourceDescription", glassDTO.SourceDescription);
                _ = cmd.Parameters.AddWithValue("@SourceColor", glassDTO.SourceColor);
                _ = cmd.Parameters.AddWithValue("@SourceType", glassDTO.Type.ToString());
                _ = cmd.Parameters.AddWithValue("@ModifiedUTCDateTime", DateTime.UtcNow);

                /*
                  order,
                                                    worksheetName,
                                                    lineNumber,
                                                    glass.Item,
                                                    glass.SortOrder,
                                                    glass.Reference,
                                                    glass.Description,
                                                    glass.Quantity,
                                                    glass.Width,
                                                    glass.Height,
                                                    glass.Weight,
                                                    glass.Area,
                                                    glass.Price,
                                                    glass.TotalPrice,
                                                    glass.SquareMeterPrice,
                                                    glass.TotalWeight,
                                                    glass.TotalArea,
                                                    glass.AreaUsed,
                                                    glass.Ordered,
                                                    glass.Waste,
                                                    glass.Pallet,
                                                    glass.SourceReference,
                                                    glass.SourceDescription,
                                                    glass.Type.ToString());
                                                    
                 */


                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result;
            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: inserting Sapa v2 glass to DB");
                return -1;
            }
        }

        public async Task<int> InsertPanelAsync(PanelDTO panelDTO)
        {
            try
            {
                if (panelDTO == null)
                {
                    _logService.Debug("Error Inserting Sapa v2 panelDTO to DB, panelDTO is null");
                    throw new ArgumentNullException(nameof(panelDTO));
                }


                SqlCommand cmd = new()
                {
                    CommandText = "Uniwave_a2pInsertMNRecordPanels",
                    CommandType = CommandType.StoredProcedure
                };


                _ = cmd.Parameters.AddWithValue("@Order", panelDTO.Order);
                _ = cmd.Parameters.AddWithValue("@Item", panelDTO.Item);
                _ = cmd.Parameters.AddWithValue("@SortOrder", panelDTO.SortOrder);
                _ = cmd.Parameters.AddWithValue("@Reference", panelDTO.Reference);
                _ = cmd.Parameters.AddWithValue("@Color", panelDTO.Color);
                _ = cmd.Parameters.AddWithValue("@CustomField1", panelDTO.CustomField1);
                _ = cmd.Parameters.AddWithValue("@CustomField2", panelDTO.CustomField2);
                _ = cmd.Parameters.AddWithValue("@CustomField3", panelDTO.CustomField3);
                _ = cmd.Parameters.AddWithValue("@Width", panelDTO.Width);
                _ = cmd.Parameters.AddWithValue("@Height", panelDTO.Height);
                _ = cmd.Parameters.AddWithValue("@Quantity", panelDTO.Quantity);
                _ = cmd.Parameters.AddWithValue("@Ordered", panelDTO.Ordered);
                _ = cmd.Parameters.AddWithValue("@AreaUsed", panelDTO.AreaUsed);
                _ = cmd.Parameters.AddWithValue("@Waste", panelDTO.Waste);
                _ = cmd.Parameters.AddWithValue("@Price", panelDTO.Price);
                _ = cmd.Parameters.AddWithValue("@CutSpecification", "Double Check");
                _ = cmd.Parameters.AddWithValue("@Modified", DateTime.UtcNow);



                int result = await _sqlRepository.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result;
            }
            catch (Exception ex)
            {
                _logService.Debug(ex.Message, "Unhandled error: inserting Sapa v2 Panel to DB");
                return -1;
            }
        }

        public Task<int> ReadMaterialAsync(MaterialDTO materialDTO)
        {
            throw new NotImplementedException();
        }
    }
}
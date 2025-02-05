using a2p.Shared.Core.DTO;
using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Repository.SubSql;
using a2p.Shared.Core.Utils;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Shared.Infrastructure.Repositories.SubSql
{
    public class SqlSapa_v2 : ISqlSapa_v2
    {
        private readonly ILogService _logger;
        private readonly ISqlService _sqlService;

        public SqlSapa_v2(ISqlService sqlService, ILogService logger)
        {
            _sqlService = sqlService;
            _logger = logger;
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
        //  _logger.Debug(ex.Message, "Unhandled error: getting color from DB");
        //  return null;
        // }
        //}

        public async Task<int> InsertItemAsync(ItemDTO item)
        {
            try
            {
                if (item == null)
                {
                    _logger.Debug("Error Inserting Sapa v2 Item to DB, item is null");
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
                _ = cmd.Parameters.AddWithValue("@Number", "");
                _ = cmd.Parameters.AddWithValue("@Version", "");
                _ = cmd.Parameters.AddWithValue("@SortOrder", item.SortOrder);
                _ = cmd.Parameters.AddWithValue("@Modified", DateTime.UtcNow);

                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result;
            }
            catch (Exception ex)
            {
                _logger.Debug(ex.Message, "Unhandled error: inserting Sapa v2 item to DB");
                return -1;
            }
        }

        public async Task<int> InsertMaterialAsync(MaterialDTO material)
        {
            try
            {
                if (material == null)
                {
                    _logger.Debug("Error Inserting Sapa v2 Material to DB, material is null");
                    throw new ArgumentNullException(nameof(material));
                }

                SqlCommand cmd = new()
                {
                    CommandText = "Uniwave_a2pInsertMNRecord",
                    CommandType = CommandType.StoredProcedure
                };

                _ = cmd.Parameters.AddWithValue("@Name", material.WorksheetName);
                _ = cmd.Parameters.AddWithValue("@Order", material.Order);
                _ = cmd.Parameters.AddWithValue("@Reference", material.Reference);
                _ = cmd.Parameters.AddWithValue("@Color", material.Color);
                _ = cmd.Parameters.AddWithValue("@CustomField1", material.CustomField1);
                _ = cmd.Parameters.AddWithValue("@CustomField2", material.CustomField2);
                _ = cmd.Parameters.AddWithValue("@CustomField3", material.CustomField3);
                _ = cmd.Parameters.AddWithValue("@Description", material.Description);
                _ = cmd.Parameters.AddWithValue("@Quantity", material.Quantity);
                _ = cmd.Parameters.AddWithValue("@PackageUnit", material.PackageUnit);
                _ = cmd.Parameters.AddWithValue("@QuantityOrdered", material.QuantityOrdered);
                _ = cmd.Parameters.AddWithValue("@QuantityRequired", material.QuantityRequired);
                _ = cmd.Parameters.AddWithValue("@Waste", material.Waste);
                _ = cmd.Parameters.AddWithValue("@Area", material.Area);
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight);
                _ = cmd.Parameters.AddWithValue("@Price", material.Price);
                _ = cmd.Parameters.AddWithValue("@TotalPrice", material.TotalPrice);
                _ = cmd.Parameters.AddWithValue("@Modified", DateTime.UtcNow);


                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result;
            }
            catch (Exception ex)
            {
                _logger.Debug(ex.Message, "Unhandled error: inserting Sapa v2 item to DB");
                return -1;
            }
        }

        public async Task<int> InsertGlassAsync(GlassDTO glass)
        {
            try
            {
                if (glass == null)
                {
                    _logger.Debug("Error Inserting Sapa v2 Glass to DB, glass is null");
                    throw new ArgumentNullException(nameof(glass));
                }

                SqlCommand cmd = new()
                {
                    CommandText = "Uniwave_a2pInsertMNRecordGlass",
                    CommandType = CommandType.StoredProcedure
                };


                _ = cmd.Parameters.AddWithValue("@Order", glass.Order);
                _ = cmd.Parameters.AddWithValue("@SortOrder", glass.SortOrder);
                _ = cmd.Parameters.AddWithValue("@Item", glass.Item);
                _ = cmd.Parameters.AddWithValue("@Reference", glass.Reference);
                _ = cmd.Parameters.AddWithValue("@Description", glass.Description);

                _ = cmd.Parameters.AddWithValue("@Quantity", glass.Quantity);
                _ = cmd.Parameters.AddWithValue("@Width", glass.Width);
                _ = cmd.Parameters.AddWithValue("@Height", glass.Height);
                _ = cmd.Parameters.AddWithValue("@Price", glass.Price);
                _ = cmd.Parameters.AddWithValue("@Weight", glass.Weight);
                _ = cmd.Parameters.AddWithValue("@Area", glass.Area);
                _ = cmd.Parameters.AddWithValue("@TotalArea", glass.TotalArea);
                _ = cmd.Parameters.AddWithValue("@AreaOrdered", glass.AreaOrdered);
                _ = cmd.Parameters.AddWithValue("@TotalPrice", glass.TotalPrice);
                _ = cmd.Parameters.AddWithValue("@Modified", DateTime.UtcNow);



                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result;
            }
            catch (Exception ex)
            {
                _logger.Debug(ex.Message, "Unhandled error: inserting Sapa v2 glass to DB");
                return -1;
            }
        }

        public async Task<int> InsertPanelAsync(PanelDTO panel)
        {
            try
            {
                if (panel == null)
                {
                    _logger.Debug("Error Inserting Sapa v2 Panel to DB, panel is null");
                    throw new ArgumentNullException(nameof(panel));
                }


                SqlCommand cmd = new()
                {
                    CommandText = "Uniwave_a2pInsertMNRecordPanels",
                    CommandType = CommandType.StoredProcedure
                };


                _ = cmd.Parameters.AddWithValue("@Order", panel.Order);
                _ = cmd.Parameters.AddWithValue("@Item", panel.Item);
                _ = cmd.Parameters.AddWithValue("@SortOrder", panel.SortOrder);
                _ = cmd.Parameters.AddWithValue("@Reference", panel.Reference);
                _ = cmd.Parameters.AddWithValue("@Color", panel.Color);
                _ = cmd.Parameters.AddWithValue("@CustomField1", panel.CustomField1);
                _ = cmd.Parameters.AddWithValue("@CustomField2", panel.CustomField2);
                _ = cmd.Parameters.AddWithValue("@CustomField3", panel.CustomField3);
                _ = cmd.Parameters.AddWithValue("@Width", panel.Width);
                _ = cmd.Parameters.AddWithValue("@Height", panel.Height);
                _ = cmd.Parameters.AddWithValue("@Quantity", panel.Quantity);
                _ = cmd.Parameters.AddWithValue("@AreaOrdered", panel.AreaOrdered);
                _ = cmd.Parameters.AddWithValue("@AreaUsed", panel.AreaUsed);
                _ = cmd.Parameters.AddWithValue("@Waste", panel.Waste);
                _ = cmd.Parameters.AddWithValue("@Price", panel.Price);
                _ = cmd.Parameters.AddWithValue("@CutSpecification", "Double Check");
                _ = cmd.Parameters.AddWithValue("@Modified", DateTime.UtcNow);



                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result;
            }
            catch (Exception ex)
            {
                _logger.Debug(ex.Message, "Unhandled error: inserting Sapa v2 Panel to DB");
                return -1;
            }
        }

        public Task<int> ReadMaterialAsync(MaterialDTO material)
        {
            throw new NotImplementedException();
        }
    }
}
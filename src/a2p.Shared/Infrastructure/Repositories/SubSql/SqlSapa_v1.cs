using System.Data;

using a2p.Shared.Core.Interfaces.Repository;
using a2p.Shared.Core.Interfaces.Repository.SubSql;
using a2p.Shared.Core.Utils;

using Microsoft.Data.SqlClient;


namespace a2p.Shared.Infrastructure.Repositories.SubSql

{
 public class SqlSapa_v1 : ISqlSapa_v1
 {

  private readonly ISqlService _sqlService;
  private readonly ILogService _logger;

  public SqlSapa_v1(ISqlService sqlService, ILogService logger)
  {
   _sqlService=sqlService;
   _logger=logger;


  }


  //public async Task<string?>? GetColorAsync(string color)
  //{
  // string sql = "SELECT Color FROM Colors WHERE Color = @Color";
  // var parameters = new[] { new SqlParameter("@Color", color) };

  // object result = await _sqlService.ExecuteScalarAsync(sql, CommandType.Text, parameters);
  // if (result == null)
  // {
  //  _logger.Debug("SS2: Error getting color from DB, result is null. SQL: ${sql}", sql);
  //  return null;
  // }
  // return result.ToString();
  //}


  //public async Task<string?>? GetGlassByDescriptionAsync(string description)
  //{
  // string sql = "SELECT RefereniaBase FROM MaterialesBase WHERE Descripcion = @description";
  // var parameters = new[] { new SqlParameter("@description", description) };

  // object result = await _sqlService.ExecuteScalarAsync(sql, CommandType.Text, parameters);
  // if (result == null)
  // {
  //  _logger.Debug("SS2: Error getting glass by description from DB, result is null. SQL: ${sql}", sql);
  //  return null;
  // }
  // return result.ToString();
  //}

  // public async Task<string?>? GetGlassByReferenceAsync(string reference)
  //{
  // string sql = "SELECT RefereniaBase FROM MaterialesBase WHERE RferenciaBase = @reference";
  // var parameters = new[] { new SqlParameter("@description", reference) };

  // object result = await _sqlService.ExecuteScalarAsync(sql, CommandType.Text, parameters);
  // if (result == null)
  // {
  //  _logger.Debug("SS2: Error getting glass by description from DB, result is null. SQL: ${sql}", sql);
  //  return null;
  // }
  // return result.ToString();
  //}


  //public async Task<(int, int)?> GetSalesDocument(string orderNumber)
  //{
  // string sql = "SELECT Number, Version FROM vwSales WHERE InternalOrderCode = @orderNumber";
  // var parameters = new[] { new SqlParameter("@orderNumber", orderNumber) };

  // var result = await _sqlService.ExecuteQueryForTwoValuesAsync<int, int>(sql, CommandType.Text, parameters);
  // if (result == (0, 0))
  // {
  //  _logger.Debug("SS2: Error getting sales document from DB, result is null. SQL: {sql}", sql);
  //  return null;
  // }
  // return result;
  //}



  //public async Task<int> InsertItemAsync(ItemDTO item)
  //{
  // try
  // {

  //  if (item == null)
  //  {
  //   _logger.Debug("Error Inserting Sapa v1 Item to DB, item is null");
  //   throw new ArgumentNullException(nameof(item));
  //  }
  //  string sql = $"EXEC Uniwave_SAPAInsertPositions " +


  //   $"sOrder = '{item.Order}', " +
  //   $"sPhase = 'AAA', " + // TO DO: Add phase
  //   $"sProduct = '{item}', " +
  //   $"sQuantity = '{item.Quantity}', " +
  //   $"sWidth = '{item.Width}', " +
  //   $"sHeight = '{item.Height}', " +
  //   $"sWeight = '{item.Weight}', " +
  //   $"sWeightWOGlass = '{item.WeightWithoutGlass}', " +
  //   $"sDirectMtrl = '{item.MaterialCost}', " +
  //   $"sDirectLW = '{item.LaborCost}', " +
  //   $"sPrice = '{item.Price}', " +
  //   $"sTotal = '{item.TotalPrice}', " +
  //   $"sPriceEUR = '{item.PriceEUR}', " +
  //   $"stotalEUR = '{item.TotalPriceEUR}', " +
  //   $"sdirectMtrlEUR = '{item.MaterialCostEUR}', " +
  //   $"sdirectLWEUR = '{item.LaborCostEUR}', " +
  //   $"sdNumber = '{""}', " +
  //   $"sdVersion = '{""}', " +
  //   $"sdSortOrder = '{""}', " +
  //   $"sModified = '{DateTime.UtcNow}'";

  //  int result = await _sqlService.ExecuteNonQueryAsync(sql);
  //  return result;
  // }
  // catch (Exception ex)
  // {
  //  _logger.Debug(ex.Message, "Unhandled error: inserting Sapa v1 item to DB");
  //  return -1;
  // }
  //}

  //public async Task<int> InsertMaterialAsync(MaterialDTO material)
  //{
  // try
  // {

  //  if (material == null)
  //  {
  //   _logger.Debug("Error Inserting Sapa v1 Material to DB, material is null");
  //   throw new ArgumentNullException(nameof(material));
  //  }

  //  string sql = $"EXEC Uniwave_SAPAInsertMNRecord " +
  //      $"@sWName = '{material.WorksheetName}', " +
  //      $"@sOrder = '{material.Order}', " +
  //      $"@sReference = '{material.Reference}', " +
  //      $"@color = '{material.Color}', " +
  //      $"@sC1 = '{material.CustomField1}', " +
  //      $"@sC2 = '{material.CustomField2}', " +
  //      $"@sC3 = '{material.CustomField3}', " +
  //      $"@sDescription = '{material.Description}', " +
  //      $"@sQuantity = {material.Quantity}, " +
  //      $"@sPackage = '{material.PackageUnit}', " +
  //      $"@sGross = {material.QuantityOrdered}, " +
  //      $"@sNet = {material.QuantityRequired}, " +
  //      $"@sExcahnge = {material.Waste}, " +
  //      $"@sm2 = {material.Area}, " +
  //      $"@sWeight = {material.Weight}, " +
  //      $"@sPrice = {material.Price}, " +
  //      $"@sTotalPrice = {material.TotalPrice}, " +
  //      $"@sModified = '{DateTime.Now}'";


  //  if (sql == null)

  //  {
  //   _logger.Debug("Error Inserting Sapa v1 Material to database, sql is null");
  //   throw new ArgumentNullException(nameof(sql));
  //  }
  //  int result = await _sqlService.ExecuteNonQueryAsync(sql);

  //  return result;

  // }
  // catch (Exception ex)
  // {
  //  _logger.Error(ex.Message, "Unhandled error: inserting Sapa v1 material to DB");
  //  return -1;
  // }
  //}

  //public async Task<int> InsertGlassAsync(GlassDTO glass)
  //{
  // try
  // {
  //  if (glass == null)
  //  {
  //   _logger.Debug("Error Inserting Sapa v1 glass to database, material is null");
  //   throw new ArgumentNullException(nameof(glass));
  //  }



  //  string sql = $"EXEC Uniwave_SAPAInsertMNRecordGlass " +
  //      $"@sOrder = '{glass.Order}', " +
  //      $"@sId = '{glass.SortOrder}', " +
  //      $"@sLineId = {glass.Item}, " +
  //      $"@sProduct = '{glass.Item}', " +
  //      $"@sReference = '{glass.Reference}', " +
  //      $"@sDescription = '{glass.Description}', " +
  //      $"@sQuantity = {glass.Quantity}, " +
  //      $"@sWidth = {Math.Floor(glass.Width)}, " +
  //      $"@sHeight = {Math.Floor(glass.Height)}, " +
  //      $"@sPrice = {glass.Price}, " +
  //      $"@sWeight = {glass.Weight}, " +
  //      $"@sm2pcs = {glass.Area}, " +
  //      $"@sm2Sum = {glass.TotalArea}, " +
  //      $"@sm2Total = {glass.AreaOrdered}, " +
  //      $"@sTotalPrice = {glass.TotalPrice}, " +
  //      $"@sModified = '{DateTime.Now}'";



  //  int result = await _sqlService.ExecuteNonQueryAsync(sql);
  //  return result;
  // }
  // catch (Exception ex)
  // {
  //  _logger.Debug(ex.Message, "Unhandled error: inserting Sapa v1 glass to DB");
  //  return -1;
  // }

  //}

  //public async Task<int> InsertPanelAsync(PanelDTO panel)
  //{
  // try
  // {
  //  if (panel == null)
  //  {
  //   _logger.Debug("Error Inserting Sapa v1 panel to database, material is null");
  //   throw new ArgumentNullException(nameof(panel));
  //  }

  //  string sql = $"EXEC Uniwave_SAPAInsertMNRecordPanels " +
  //   $"sOrder = '{panel.Order}', " +
  //   $"sId = '{panel}', " +
  //   $"sLineId = '{panel.SortOrder}', " +
  //   $"sReference = '{panel.Reference}', " +
  //   $"sColor = '{panel.Color}', " +
  //   $"sC1 = '{panel.CustomField1}', " +
  //   $"sC2 = '{panel.CustomField2}', " +
  //   $"sC3 = '{panel.CustomField3}', " +
  //   $"sWidth = '{panel.Width}', " +
  //   $"sHeight = '{panel.Height}', " +
  //   $"sQuantity = '{panel.Quantity}', " +
  //   $"sTotalArea = '{panel.AreaOrdered}', " +
  //   $"sUsedArea = '{panel.AreaUsed}', " +
  //   $"sTotalWaste = '{panel.Waste}', " +
  //   $"sPrice = '{panel.Price}', " +
  //   $"sCutSpecification = '{"Double Check"}', " +
  //   $"sModified = '{DateTime.UtcNow}'";

  //  await _sqlService.ExecuteNonQueryAsync(sql);


  //  int result = await _sqlService.ExecuteNonQueryAsync(sql);
  //  return result;
  // }
  // catch (Exception ex)
  // {
  //  _logger.Error(ex.Message, "Unhandled error: inserting Sapa v1 panel to DB");
  //  return -1;
  // }
  //}


  //Examples
  //================================================\


  public async Task<DataTable> GetUsersAsync()
  {
   string sql = "SELECT * FROM Users";
   return await _sqlService.ExecuteQueryAsync(sql, CommandType.Text);
  }

  public async Task<int> UpdateUserAsync(int userId, string newName)
  {
   string sql = "UPDATE Users SET Name = @Name WHERE Id = @Id";
   SqlParameter[] parameters = new[]
   {
   new SqlParameter("@Name", newName),
   new SqlParameter("@Id", userId)
  };

   return await _sqlService.ExecuteNonQueryAsync(sql, CommandType.Text, parameters);
  }

  public async Task<int> CallStoredProcedureAsync(string procedureName, SqlParameter[] parameters)
  {
   return await _sqlService.ExecuteNonQueryAsync(procedureName, CommandType.StoredProcedure, parameters);
  }




 }
}

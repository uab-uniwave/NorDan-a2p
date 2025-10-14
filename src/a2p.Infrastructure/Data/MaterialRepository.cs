// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Interfaces;
using a2p.Domain.Entities;
using a2p.Domain.Enums;
using a2p.Domain.Interfaces;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Infrastructure.Data
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly ISQLService _sqlService;

        public MaterialRepository(ISQLService sqlService)
        {
            _sqlService = sqlService ?? throw new ArgumentNullException(nameof(sqlService));
        }

        public async Task<MaterialEntity?> InsertMaterialAsync(MaterialEntity material)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "INSERT INTO [dbo].[Uniwave_a2p_Materials] " +
                    "(" +
                    "[RowId]," + //required
                    "[SalesDocumentNumber]," + //required
                    "[SalesDocumentVersion]," + //required
                    "[OrderNumber]," + //required
                    "[Worksheet]," + //required
                    "[Line]," + //required
                    "[Column]," + //required
                    "[Item]," +
                    "[SortOrder]," +
                    "[ReferenceBase]," +
                    "[Reference]," +
                    "[Description]," +
                    "[Color]," +
                    "[ColorDescription]," +
                    "[Width]," +
                    "[Height]," +
                    "[Quantity]," +
                    "[PackageQuantity]," +
                    "[TotalQuantity]," +
                    "[RequiredQuantity]," +
                    "[LeftOverQuantity]," +
                    "[Weight]," +
                    "[TotalWeight]," +
                    "[RequiredWeight]," +
                    "[LeftOverWeight]," +
                    "[Area]," +
                    "[TotalArea]," +
                    "[RequiredArea]," +
                    "[LeftOverArea]," +
                    "[Waste]," +
                    "[Price]," +
                    "[TotalPrice]," +
                    "[RequiredPrice]," +
                    "[LeftOverPrice]," +
                    "[SquareMeterPrice]," +
                    "[Pallet]," +
                    "[MaterialType]," +
                    "[CustomField1]," +
                    "[CustomField2]," +
                    "[CustomField3]," +
                    "[CustomField4]," +
                    "[CustomField5]," +
                    "[SourceReference]," +
                    "[SourceDescription]," +
                    "[SourceColor]," +
                    "[SourceColorDescription]," +
                    "[CreatedUTCDateTime]," +
                    "[ModifiedUTCDateTime]" +
                    ") " +
                    "VALUES " +
                    "(" +
                    "@RowId," + //required
                    "@SalesDocumentNumber," + //required
                    "@SalesDocumentVersion," + //required
                    "@OrderNumber," + //required
                    "@Worksheet," + //required

                    "@Line," + //required
                    "@Column," + //required
                    "@Item," +
                    "@SortOrder," +
                    "@ReferenceBase," +
                    "@Reference," +
                    "@Description," +
                    "@Color," +
                    "@ColorDescription," +
                    "@Width," +
                    "@Height," +
                    "@Quantity," +
                    "@PackageQuantity," +
                    "@TotalQuantity," +
                    "@RequiredQuantity," +
                    "@LeftOverQuantity," +
                    "@Weight," +
                    "@TotalWeight," +
                    "@RequiredWeight," +
                    "@LeftOverWeight," +
                    "@Area," +
                    "@TotalArea," +
                    "@RequiredArea," +
                    "@LeftOverArea," +
                    "@Waste," +
                    "@Price," +
                    "@TotalPrice," +
                    "@RequiredPrice," +
                    "@LeftOverPrice," +
                    "@SquareMeterPrice," +
                    "@Pallet," +
                    "@MaterialType," +
                    "@CustomField1," +
                    "@CustomField2," +
                    "@CustomField3," +
                    "@CustomField4," +
                    "@CustomField5," +
                    "@SourceReference," +
                    "@SourceDescription," +
                    "@SourceColor," +
                    "@SourceColorDescription," +
                    "@CreatedUTCDateTime," +
                    "@ModifiedUTCDateTime" +
                    ")"
                  ,
                    CommandType = CommandType.Text
                };

                // Create parameters for the insert
                var parameters = CreateMaterialParameters(material);
                await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                // Query to get the inserted material
                cmd.CommandText = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Materials] WHERE [RowId] = @RowId";
                var queryParams = new SqlParameter[]
                {
                    new SqlParameter("@RowId", material.RowId)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, queryParams);
                if (result == null)
                    return null;
                return MapDataRowToMaterial(result.Rows[0]);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                //_logService.Error(sqlEx, "SQL Error Inserting Material {0}-{1} in Order {2}.", material.Reference, material.Color, material.OrderNumber);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //  _logService.Error(ex, "Error Inserting Material {0}-{1} in Order {2}.", material.Reference, material.Color, material.OrderNumber);
                return null;
            }
        }

        public async Task<MaterialEntity?> UpdateMaterialAsync(MaterialEntity material)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "UPDATE [dbo].[Uniwave_a2p_Materials] " +
                    "SET " +
                    "[SalesDocumentNumber] = @SalesDocumentNumber, " +
                    "[SalesDocumentVersion] = @SalesDocumentVersion, " +
                    "[OrderNumber] = @OrderNumber, " +
                    "[Worksheet] = @Worksheet, " +
                    "[Line] = @Line, " +
                    "[Column] = @Column, " +
                    "[Item] = @Item, " +
                    "[SortOrder] = @SortOrder, " +
                    "[ReferenceBase] = @ReferenceBase, " +
                    "[Reference] = @Reference, " +
                    "[Description] = @Description, " +
                    "[Color] = @Color, " +
                    "[ColorDescription] = @ColorDescription, " +
                    "[Width] = @Width, " +
                    "[Height] = @Height, " +
                    "[Quantity] = @Quantity, " +
                    "[PackageQuantity] = @PackageQuantity, " +
                    "[TotalQuantity] = @TotalQuantity, " +
                    "[RequiredQuantity] = @RequiredQuantity, " +
                    "[LeftOverQuantity] = @LeftOverQuantity, " +
                    "[Weight] = @Weight, " +
                    "[TotalWeight] = @TotalWeight, " +
                    "[RequiredWeight] = @RequiredWeight, " +
                    "[LeftOverWeight] = @LeftOverWeight, " +
                    "[Area] = @Area, " +
                    "[TotalArea] = @TotalArea, " +
                    "[RequiredArea] = @RequiredArea, " +
                    "[LeftOverArea] = @LeftOverArea, " +
                    "[Waste] = @Waste, " +
                    "[Price] = @Price, " +
                    "[TotalPrice] = @TotalPrice, " +
                    "[RequiredPrice] = @RequiredPrice, " +
                    "[LeftOverPrice] = @LeftOverPrice, " +
                    "[SquareMeterPrice] = @SquareMeterPrice, " +
                    "[Pallet] = @Pallet, " +
                    "[MaterialType] = @MaterialType, " +
                    "[CustomField1] = @CustomField1, " +
                    "[CustomField2] = @CustomField2, " +
                    "[CustomField3] = @CustomField3, " +
                    "[CustomField4] = @CustomField4, " +
                    "[CustomField5] = @CustomField5, " +
                    "[SourceReference] = @SourceReference, " +
                    "[SourceDescription] = @SourceDescription, " +
                    "[SourceColor] = @SourceColor, " +
                    "[SourceColorDescription] = @SourceColorDescription, " +
                    "[ModifiedUTCDateTime] = @ModifiedUTCDateTime " +
                    "WHERE [RowId] = @RowId",
                    CommandType = CommandType.Text
                };

                // Create parameters for the update
                var parameters = CreateMaterialParameters(material);
                await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                // Query to get the updated material
                cmd.CommandText = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Materials] WHERE [RowId] = @RowId";
                var queryParams = new SqlParameter[]
                {
                    new SqlParameter("@RowId", material.RowId)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, queryParams);
                if (result == null)
                    return null;
                return MapDataRowToMaterial(result.Rows[0]);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                // _logService.Error(sqlEx, "SQL Error Updating Material {0}-{1} in Order {2}.", material.Reference, material.Color, material.OrderNumber);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // _logService.Error(ex, "Error Updating Material {0}-{1} in Order {2}.", material.Reference, material.Color, material.OrderNumber);
                return null;
            }
        }

        public async Task<MaterialEntity?> GetMaterialAsync(Guid id)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT TOP 1 * FROM [dbo].[Uniwave_a2p_Materials] WHERE [RowId] = @RowId",
                    CommandType = CommandType.Text
                };

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@RowId", id)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                if (result == null || result.Rows.Count == 0)
                    return null;

                return MapDataRowToMaterial(result.Rows[0]);
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<MaterialEntity>?> GetOrderMaterialsAsync(Guid orderId)
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT * FROM [dbo].[Uniwave_a2p_Materials] WHERE [OrderId] = @OrderId",
                    CommandType = CommandType.Text
                };

                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@OrderId", orderId)
                };

                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType, parameters);

                if (result == null || result.Rows.Count == 0)
                    return Array.Empty<MaterialEntity>();

                List<MaterialEntity> materials = new();
                foreach (DataRow row in result.Rows)
                {
                    var material = MapDataRowToMaterial(row);
                    if (material != null)
                        materials.Add(material);
                }

                return materials;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                return Array.Empty<MaterialEntity>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Array.Empty<MaterialEntity>();
            }
        }

        public async Task<IEnumerable<MaterialEntity>?> GetMaterialsAsync()
        {
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT * FROM [dbo].[Uniwave_a2p_Materials]",
                    CommandType = CommandType.Text
                };
                var result = await _sqlService.ExecuteQueryAsync(cmd.CommandText, cmd.CommandType);

                if (result == null || result.Rows.Count == 0)
                    return Array.Empty<MaterialEntity>();

                List<MaterialEntity> materials = new();
                foreach (DataRow row in result.Rows)
                {
                    var material = MapDataRowToMaterial(row);
                    if (material != null)
                        materials.Add(material);
                }
                return materials;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine(sqlEx.Message);
                return Array.Empty<MaterialEntity>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return Array.Empty<MaterialEntity>();
            }
        }

        private static MaterialEntity? MapDataRowToMaterial(DataRow row)
        {
            try
            {
                if (row == null)
                    return null;

                return new MaterialEntity
                {
                    // Base entity properties
                    RowId = row.Field<Guid>("RowId"),

                    // Order related properties
                    OrderId = row.Field<Guid>("OrderId"),
                    OrderNumber = row.Field<string>("OrderNumber") ?? string.Empty,
                    Worksheet = row.Field<string>("Worksheet") ?? string.Empty,
                    Line = row.Field<int>("Line"),
                    Column = row.Field<int>("Column"),

                    // Item identification
                    ItemName = row.Field<string>("Item"),
                    ItemId = row.Field<Guid?>("ItemId"),
                    SortOrder = row.Field<int>("SortOrder"),

                    // Reference information
                    ReferenceBase = row.Field<string>("ReferenceBase") ?? string.Empty,
                    Reference = row.Field<string>("Reference") ?? string.Empty,
                    Description = row.Field<string>("Description"),

                    // Color information
                    Color = row.Field<string>("Color") ?? string.Empty,
                    ColorDescription = row.Field<string>("ColorDescription") ?? string.Empty,

                    // Dimensions
                    Width = row.Field<decimal>("Width"),
                    Height = row.Field<decimal>("Height"),

                    // Quantity information
                    Quantity = row.Field<int>("Quantity"),
                    PackageQuantity = row.Field<decimal>("PackageQuantity"),
                    TotalQuantity = row.Field<decimal>("TotalQuantity"),
                    RequiredQuantity = row.Field<decimal>("RequiredQuantity"),
                    LeftOverQuantity = row.Field<decimal>("LeftOverQuantity"),

                    // Weight information
                    Weight = row.Field<decimal>("Weight"),
                    TotalWeight = row.Field<decimal>("TotalWeight"),
                    RequiredWeight = row.Field<decimal>("RequiredWeight"),
                    LeftOverWeight = row.Field<decimal>("LeftOverWeight"),

                    // Area information
                    Area = row.Field<decimal>("Area"),
                    TotalArea = row.Field<decimal>("TotalArea"),
                    RequiredArea = row.Field<decimal>("RequiredArea"),
                    LeftOverArea = row.Field<decimal>("LeftOverArea"),
                    Waste = row.Field<decimal>("Waste"),

                    // Price information
                    Price = row.Field<decimal>("Price"),
                    TotalPrice = row.Field<decimal>("TotalPrice"),
                    RequiredPrice = row.Field<decimal>("RequiredPrice"),
                    LeftOverPrice = row.Field<decimal>("LeftOverPrice"),
                    SquareMeterPrice = row.Field<decimal>("SquareMeterPrice"),

                    // Additional information
                    Pallet = row.Field<string>("Pallet"),
                    MaterialType = (MaterialType)row.Field<int>("MaterialType"),

                    // Custom fields
                    CustomField1 = row.Field<string>("CustomField1"),
                    CustomField2 = row.Field<string>("CustomField2"),
                    CustomField3 = row.Field<string>("CustomField3"),
                    CustomField4 = row.Field<string>("CustomField4"),
                    CustomField5 = row.Field<string>("CustomField5"),

                    // Source information
                    SourceReference = row.Field<string>("SourceReference") ?? string.Empty,
                    SourceDescription = row.Field<string>("SourceDescription") ?? string.Empty,
                    SourceColor = row.Field<string>("SourceColor") ?? string.Empty,
                    SourceColorDescription = row.Field<string>("SourceColorDescription"),

                    // Audit fields from base entity
                    CreatedUTCDateTime = row.Field<DateTime>("CreatedUTCDateTime"),
                    ModifiedUTCDateTime = row.Field<DateTime>("ModifiedUTCDateTime")
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error mapping DataRow to MaterialEntity: {ex.Message}");
                return null;
            }
        }

        private static SqlParameter[] CreateMaterialParameters(MaterialEntity material)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@RowId", material.RowId),
                new SqlParameter("@OrderId", material.OrderId),
                new SqlParameter("@OrderNumber", material.OrderNumber),
                new SqlParameter("@Worksheet", material.Worksheet),
                new SqlParameter("@Line", material.Line),
                new SqlParameter("@Column", material.Column),
                new SqlParameter("@Item", material.ItemName ?? (object)DBNull.Value),
                new SqlParameter("@ItemId", material.ItemId ?? (object)DBNull.Value),
                new SqlParameter("@SortOrder", material.SortOrder),
                new SqlParameter("@ReferenceBase", material.ReferenceBase),
                new SqlParameter("@Reference", material.Reference),
                new SqlParameter("@Description", material.Description ?? (object)DBNull.Value),
                new SqlParameter("@Color", material.Color),
                new SqlParameter("@ColorDescription", material.ColorDescription),
                new SqlParameter("@Width", material.Width),
                new SqlParameter("@Height", material.Height),
                new SqlParameter("@Quantity", material.Quantity),
                new SqlParameter("@PackageQuantity", material.PackageQuantity),
                new SqlParameter("@TotalQuantity", material.TotalQuantity),
                new SqlParameter("@RequiredQuantity", material.RequiredQuantity),
                new SqlParameter("@LeftOverQuantity", material.LeftOverQuantity),
                new SqlParameter("@Weight", material.Weight),
                new SqlParameter("@TotalWeight", material.TotalWeight),
                new SqlParameter("@RequiredWeight", material.RequiredWeight),
                new SqlParameter("@LeftOverWeight", material.LeftOverWeight),
                new SqlParameter("@Area", material.Area),
                new SqlParameter("@TotalArea", material.TotalArea),
                new SqlParameter("@RequiredArea", material.RequiredArea),
                new SqlParameter("@LeftOverArea", material.LeftOverArea),
                new SqlParameter("@Waste", material.Waste),
                new SqlParameter("@Price", material.Price),
                new SqlParameter("@TotalPrice", material.TotalPrice),
                new SqlParameter("@RequiredPrice", material.RequiredPrice),
                new SqlParameter("@LeftOverPrice", material.LeftOverPrice),
                new SqlParameter("@SquareMeterPrice", material.SquareMeterPrice),
                new SqlParameter("@Pallet", material.Pallet ?? (object)DBNull.Value),
                new SqlParameter("@MaterialType", (int)material.MaterialType),
                new SqlParameter("@CustomField1", material.CustomField1 ?? (object)DBNull.Value),
                new SqlParameter("@CustomField2", material.CustomField2 ?? (object)DBNull.Value),
                new SqlParameter("@CustomField3", material.CustomField3 ?? (object)DBNull.Value),
                new SqlParameter("@CustomField4", material.CustomField4 ?? (object)DBNull.Value),
                new SqlParameter("@CustomField5", material.CustomField5 ?? (object)DBNull.Value),
                new SqlParameter("@SourceReference", material.SourceReference),
                new SqlParameter("@SourceDescription", material.SourceDescription),
                new SqlParameter("@SourceColor", material.SourceColor),
                new SqlParameter("@SourceColorDescription", material.SourceColorDescription ?? (object)DBNull.Value),
                new SqlParameter("@CreatedUTCDateTime", material.CreatedUTCDateTime),
                new SqlParameter("@ModifiedUTCDateTime", material.ModifiedUTCDateTime)
            };
        }
    }
}





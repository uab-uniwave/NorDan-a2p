// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Interfaces;
using a2p.Domain.Entities;

using Microsoft.Data.SqlClient;

using System.Data;

namespace a2p.Infrastructure.Services.PrefSuiteService
{
    public class PrefSuiteDataService : IPrefSuiteDataService
    {
        private readonly ILogService _logService;
        private readonly ISQLService _sqlService;

        public PrefSuiteDataService(ISQLService sqlService, ILogService logService)
        {
            _sqlService = sqlService ?? throw new ArgumentNullException(nameof(sqlService));
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }

        public async Task<int> GetSalesDocumentStateAsync(int number, int version)
        {

            object? result;
            int state = 0;

            if (number < 1 || version < 1)
            {
                _logService.Verbose("{$Class}.{$Method}. Error getting sales document state. Number {$Number} or version {$Version} are wrong.",
                       nameof(PrefSuiteDataService),
                nameof(GetSalesDocumentStateAsync), number, version);
                return state;
            }

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT [dbo].[Uniwave_a2p_GetSalesDocumentState](@Number, @Version)",
                };

                _ = cmd.Parameters.AddWithValue("@Number", number);
                _ = cmd.Parameters.AddWithValue("@Version", version);

                result = await _sqlService.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                state = result != DBNull.Value ? (int)result! : 0;

                return state;

            }

            catch (Exception ex)
            {
                _logService.Verbose(
                "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}. Error getting order state for sales document. Exception: {Exception}.",
                nameof(PrefSuiteDataService),
                nameof(GetSalesDocumentStateAsync),
                 ex.Message
               );
                return state;
            }

        }

        public async Task<(int?, int?)> GetSalesDocumentAsync(string order)
        {
            (int?, int?) result;

            if (string.IsNullOrEmpty(order))
            {

                return (-1, -1);

            }
            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = $"SELECT TOP 1 [Numero], [Version] FROM PAF WHERE Referencia like N'{order}%'"
                };

                result = await _sqlService.ExecuteQueryTupleValuesAsync(cmd.CommandText, cmd.CommandType);
                return result.Item1 < 1 || result.Item2 < 1 ? (-1, -1) : result;

            }

            catch (Exception ex)
            {
                _logService.Verbose(
                "{$Class}.{$Method}. Unhandled error getting sales document number and version. Exception: {Exception}.",
                nameof(PrefSuiteDataService),
                nameof(GetSalesDocumentAsync),
                ex.Message
               );
                return (-1, -1);

            }

        }
        public async Task<string?> GetGlassReferenceAsync(string description)
        {

            string? glassReference = null;

            if (string.IsNullOrEmpty(description))
            {

                _logService.Information("{$Class}.{$Method}. Error getting glass reference. Provided glass description is missing.",
                  nameof(PrefSuiteDataService),
                      nameof(GetGlassReferenceAsync));
                return null;
            }

            try
            {
                string sqlCommand = $"SELECT TOP 1 ReferenciaBase FROM MaterialesBase WHERE tipocalculo = 'Superficies' and Nivel1 = '03 Glass' and Descripcion = '{description}'";
                CommandType commandType = CommandType.Text;
                object? result = await _sqlService.ExecuteScalarAsync(sqlCommand, commandType);

                if (result == null)
                {
                    _logService.Verbose("{$Class}.{$Method}. Error getting glass reference. Glass with description {$Description} not found coresponding glass reference in PrefSuite DB.",
                      nameof(PrefSuiteDataService),
                      nameof(GetGlassReferenceAsync),
                      description);
                    return null;
                }

                glassReference = result.ToString();

                if (string.IsNullOrEmpty(glassReference))
                {
                    _logService.Verbose("{$Class}.{$Method}. Error getting glass reference. Glass with description {$Description} not found coresponding glass reference in PrefSuite DB.",
                      nameof(PrefSuiteDataService),
                      nameof(GetGlassReferenceAsync),
                      description);
                    return null;
                }

                _logService.Verbose
                    ("{$Class}.{$Method}. Glass with description {$Description}  found coresponding glass reference {$Reference} in PrefSuite DB.",
                     description,
                     glassReference,
                     nameof(PrefSuiteDataService),
                     nameof(GetGlassReferenceAsync),
                     description);

                return glassReference;

            }
            catch (Exception ex)
            {
                _logService.Verbose(
                  "{$Class}.{$Method}. Unhandled error inserting color configuration for color {$Color}. Exception: {$Exception}.",
                  nameof(PrefSuiteDataService),
                  nameof(GetGlassReferenceAsync),
                  ex.Message
                 );

                return glassReference;

            }
        }
        public async Task<int?> GetCommodityCode(string sourceReference)
        {
            if (string.IsNullOrEmpty(sourceReference))
            {
                _logService.Information("{$Class}.{$Method}. Error getting TechDesign commodity code. Provided sourceReference is missing.",
                    nameof(PrefSuiteDataService),
                    nameof(GetCommodityCode));
                return null;
            }

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT [dbo].[Uniwave_a2p_GetTechDesignCommodityCode](@SourceReference)",
                };

                _ = cmd.Parameters.AddWithValue("@SourceReference", sourceReference);

                object? result = await _sqlService.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                return result != null && result != DBNull.Value ? (int)result : null;
                ;
            }
            catch (Exception ex)
            {
                _logService.Verbose(
                    "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}. Error getting TechDesign commodity code. Exception: {Exception}.",
                    nameof(PrefSuiteDataService),
                    nameof(GetCommodityCode),
                    ex.Message
                );
                return null;
            }
        }

        public async Task<decimal> GetTechDesignWeight(string sourceReference)
        {
            if (string.IsNullOrEmpty(sourceReference))
            {
                _logService.Information("{$Class}.{$Method}. Error getting TechDesign Weight. Provided sourceReference is missing.",
                    nameof(PrefSuiteDataService),
                    nameof(GetTechDesignWeight));
                return 0;
            }

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT [dbo].[Uniwave_a2p_GetTechDesignWeight](@SourceReference)",
                };

                _ = cmd.Parameters.AddWithValue("@SourceReference", sourceReference);

                object? result = await _sqlService.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result != null && result != DBNull.Value ? (decimal)result : 0;
            }
            catch (Exception ex)
            {
                _logService.Verbose(
                    "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}.Error getting TechDesign Weight. Exception: {Exception}.",
                    nameof(PrefSuiteDataService),
                    nameof(GetTechDesignWeight),
                    ex.Message
                );
                return 0;
            }
        }
        public async Task<string?> GetSapaColorAsync(string color)
        {
            if (string.IsNullOrEmpty(color))
            {
                _logService.Information("{$Class}.{$Method}. Error getting Sapa color. Provided color is missing.",
                    nameof(PrefSuiteDataService),
                    nameof(GetSapaColorAsync));
                return null;
            }

            try
            {
                SqlCommand cmd = new()
                {
                    CommandText = "SELECT [dbo].[Uniwave_a2p_GetSapaColor](@TechDesignColor)",
                };

                _ = cmd.Parameters.AddWithValue("@TechDesignColor", color);

                object? result = await _sqlService.ExecuteScalarAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                return result != null && result != DBNull.Value ? result.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                _logService.Verbose(
                    "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}. Error getting order state for sales document. Exception: {Exception}.",
                    nameof(PrefSuiteDataService),
                    nameof(GetSapaColorAsync),
                    ex.Message
                );
                return string.Empty;
            }
        }

        public async Task<int> GetPrefSuiteColorConfigurationAsync(string color)
        {
            int result = -1;

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteColorConfiguration]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Color", color); //required

                //=====================================================================================================================
                result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    //    _logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} successfully inserted into PrefSuite DB.",
                    //      nameof(PrefSuiteDataService),
                    //      nameof(GetPrefSuiteColorConfigurationAsync),
                    //      color);
                }

                if (result == 0)
                {

                    //_logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} already exists in PrefSuite DB.",
                    //  nameof(PrefSuiteDataService),
                    //  nameof(GetPrefSuiteColorConfigurationAsync),
                    // color);

                }
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);


                //_logService.Verbose(
                //"{$Class}.{$Method}. Unhandled error inserting color configuration for color {$Color}. Exception: {$Exception}.",
                //nameof(PrefSuiteDataService),
                //nameof(GetPrefSuiteColorConfigurationAsync),
                //color,
                //ex.Message
                //);
                return result;
            }

        }
        public async Task DeleteSalesDocumentDataAsync(int number, int version, bool deleteExisting)
        {

            if (number < 1 || version < 1)
            {
                //_logService.Error("{$Class}.{$Method}. Error deleting sales document data. Number {$Number} or version {$Version} are wrong.",
                // nameof(PrefSuiteDataService),
                //   nameof(DeleteSalesDocumentDataAsync),
                //   number,
                //   version);
                //return new ErrorEntity()
                //{
                //    OrderNumber = string.Empty,
                //    Level = ErrorLevel.Error,
                //    Code = ErrorCode.DatabaseWrite_Material,
                //    Message = $"Error {nameof(PrefSuiteDataService)}.{nameof(DeleteSalesDocumentDataAsync)}.  "
                //};
            }

            try
            {
                var delete = deleteExisting ? 1 : 0;

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_DeleteExistingData]",
                    CommandType = CommandType.StoredProcedure
                };

                _ = cmd.Parameters.AddWithValue("@Number", number);
                _ = cmd.Parameters.AddWithValue("@Version", version);
                _ = cmd.Parameters.AddWithValue("@DeleteExisting", delete);

                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());


            }
            catch (Exception ex)
            {
                // _logService.Verbose(
                // "{$Class}.{$Method}. Unhandled error in {$Class}. {$Method}. Error deleting sales document data for sales document {$Number}/{$Version} . Exception: {$Exception}.",
                // nameof(PrefSuiteDataService),
                // nameof(DeleteSalesDocumentDataAsync),
                // number,
                // version,
                // ex.Message
                //);
                //return new ErrorEntity()
                //{
                //    OrderNumber = string.Empty,
                //    Level = ErrorLevel.Error,
                //    Code = ErrorCode.DatabaseWrite_Material,
                //    Message = $"Error {nameof(PrefSuiteDataService)}.{nameof(DeleteSalesDocumentDataAsync)}." +
                //    $"\nError deleting sales document data for sales document {number}/{version}." +
                //    $"\n{ex.Message}.  "
                //};
            }

        }

        public async Task InsertPrefSuiteColorAsync(MaterialEntity material)
        {
            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteColor]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Color", material.Color); //required
                _ = cmd.Parameters.AddWithValue("@ColorDescription", material.ColorDescription); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Color {$Color}, {$ColorDescription} successfully inserted into PrefSuite DB.", material.Color, material.ColorDescription ?? "Without");
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Color {$Color}, {$ColorDescription} already exists in PrefSuite DB.", material.Color, material.ColorDescription ?? "Without");

                }

            }
            catch (Exception ex)
            {
                // _logService.Error(
                // "{$Class}.{$Method}. Unhandled error." +
                // "\nOrder {$OrderNumber}," +
                // "\nWorksheet {$Worksheet}," +
                // "\nLine {$Line}," +
                // "\nReferenceBase {$ReferenceBase}, " +
                // "\nReference {$Reference}," +
                // "\nColor {$Color}, " +
                // "\nColor {$ColorDescription}, " +
                // "\nDescription {$Description}," +
                // "\nException: {$Exception}",
                // nameof(PrefSuiteDataService),
                // nameof(InsertPrefSuiteColorAsync),
                // material.OrderNumber ?? string.Empty,
                // material.Worksheet ?? string.Empty,
                // material.Line,
                // material.ReferenceBase ?? string.Empty,
                // material.Reference ?? string.Empty,
                // material.Color ?? string.Empty,
                //  material.ColorDescription ?? string.Empty,
                // material.Description ?? string.Empty,
                // ex.Message ?? string.Empty
                //);

                {
                    // OrderNumber = material.OrderNumber ?? string.Empty,
                    // Level = ErrorLevel.Error,
                    // Code = ErrorCode.DatabaseWrite_Material,
                    // Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteColorAsync)}. Unhandled error." +
                    //$"\nOrder {material.OrderNumber ?? string.Empty}," +
                    //$"\nWorksheet {material.Worksheet ?? string.Empty}," +
                    //$"\nLine {material.Line}," +
                    //$"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                    //$"\nReference {material.Reference ?? string.Empty}," +
                    //$"\nColor {material.Color ?? string.Empty}, " +
                    //$"\nColorDescription {material.ColorDescription ?? string.Empty}, " +
                    //$"\nDescription {material.Description ?? string.Empty}," +
                    //$"\nException: {ex.Message ?? string.Empty}"

                }
                ;
            }

        }
        public async Task InsertPrefSuiteColorConfigurationAsync(MaterialEntity material)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteColorConfiguration]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Color", material.Color); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    _logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} successfully inserted into PrefSuite DB.", material.Color);
                }

                if (result == 0)
                {

                    _logService.Verbose("{$Class}.{$Method}. Color configuration for color {$Color} already exists in PrefSuite DB.", material.Color);

                }


            }
            catch (Exception ex)
            {
                // _logService.Error(
                // "{$Class}.{$Method}. Unhandled error." +
                // "\nOrder {$OrderNumber}," +
                // "\nWorksheet {$Worksheet}," +
                // "\nLine {$Line}," +
                // "\nReferenceBase {$ReferenceBase}, " +
                // "\nReference {$Reference}," +
                // "\nColor {$Color}, " +
                // "\nColor {$ColorDescription}, " +
                // "\nDescription {$Description}," +
                // "\nException: {$Exception}",
                // nameof(PrefSuiteDataService),
                // nameof(InsertPrefSuiteColorConfigurationAsync),
                // material.OrderNumber ?? string.Empty,
                // material.Worksheet ?? string.Empty,
                // material.Line,
                // material.ReferenceBase ?? string.Empty,
                // material.Reference ?? string.Empty,
                // material.Color ?? string.Empty,
                //  material.ColorDescription ?? string.Empty,
                // material.Description ?? string.Empty,
                // ex.Message ?? string.Empty
                //);
                //return new ErrorEntity()
                //{
                //    OrderNumber = material.OrderNumber ?? string.Empty,
                //    Level = ErrorLevel.Error,
                //    Code = ErrorCode.DatabaseWrite_Material,
                //    Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteColorConfigurationAsync)}. Unhandled error." +
                //   $"\nOrder {material.OrderNumber ?? string.Empty}," +
                //   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                //   $"\nLine {material.Line}," +
                //   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                //   $"\nReference {material.Reference ?? string.Empty}," +
                //   $"\nColor {material.Color ?? string.Empty}, " +
                //   $"\nColorDescription {material.ColorDescription ?? string.Empty}, " +
                //   $"\nDescription {material.Description ?? string.Empty}," +
                //   $"\nException: {ex.Message ?? string.Empty}"

                //};
            }

        }
        public async Task InsertPrefSuiteMaterialBaseAsync(MaterialEntity material)
        {

            try

            {

                material.CommodityCode = await GetCommodityCode(material.SourceReference ?? string.Empty);


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialBase]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Description", material.Description ?? ""); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", material.MaterialType); //required
                _ = cmd.Parameters.AddWithValue("@CommodityCode", material.CommodityCode ?? (object)DBNull.Value);

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    //_logService.Verbose("{$Class}.{$Method}. Material Base {$ReferenceBase} {$Description} successfully inserted into PrefSuite DB",
                    //    nameof(PrefSuiteDataService),
                    //    nameof(InsertPrefSuiteMaterialBaseAsync),
                    //    material.ReferenceBase,
                    //    material.Description ?? "");
                }

                if (result == 0)
                {

                    //_logService.Verbose("{$Class}.{$Method}. Material {$Reference} {$Description} already exists in PrefSuite DB.",
                    //    nameof(PrefSuiteDataService),
                    //    nameof(InsertPrefSuiteMaterialBaseAsync),
                    //    material.ReferenceBase,
                    //    material.Description ?? "");

                }


            }
            catch (Exception ex)
            {
                // _logService.Error(
                // "{$Class}.{$Method}. Unhandled error." +
                // "\nOrder {$OrderNumber}," +
                // "\nWorksheet {$Worksheet}," +
                // "\nLine {$Line}," +
                // "\nReferenceBase {$ReferenceBase}, " +
                // "\nReference {$Reference}," +
                // "\nColor {$Color}, " +
                // "\nDescription {$Description}," +
                // "\nException: {$Exception}",
                // nameof(PrefSuiteDataService),
                // nameof(InsertPrefSuiteMaterialBaseAsync),
                // material.OrderNumber ?? string.Empty,
                // material.Worksheet ?? string.Empty,
                // material.Line,
                // material.ReferenceBase ?? string.Empty,
                // material.Reference ?? string.Empty,
                // material.Color ?? string.Empty,
                // material.Description ?? string.Empty,
                // ex.Message ?? string.Empty
                //);
                //return new ErrorEntity()
                //{
                //    OrderNumber = material.OrderNumber ?? string.Empty,
                //    Level = ErrorLevel.Error,
                //    Code = ErrorCode.DatabaseWrite_Material,
                //    Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialBaseAsync)}. Unhandled error." +
                //   $"\nOrder {material.OrderNumber ?? string.Empty}," +
                //   $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                //   $"\nLine {material.Line}," +
                //   $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                //   $"\nReference {material.Reference ?? string.Empty}," +
                //   $"\nColor {material.Color ?? string.Empty}, " +
                //   $"\nDescription {material.Description ?? string.Empty}," +
                //   $"\nException: {ex.Message ?? string.Empty}"

                //};
            }

        }
        public async Task InsertPrefSuiteMaterialAsync(MaterialEntity material)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterial]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Reference", material.Reference); //required
                _ = cmd.Parameters.AddWithValue("@Color", material.Color); //required
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", material.PackageQuantity); //required
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", material.MaterialType); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                //if (result > 0)
                //{
                //    _logService.Verbose("{$Class}.{$Method}. Material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                //nameof(PrefSuiteDataService),
                //nameof(InsertPrefSuiteMaterialAsync),
                //material.Reference,
                //material.Color,
                //material.Description ?? "");
                //}

                //if (result == 0)
                //{

                //    _logService.Verbose("{$Class}.{$Method}. Material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                //nameof(PrefSuiteDataService),
                //nameof(InsertPrefSuiteMaterialAsync),
                //material.Reference, material.Color,
                //material.Description ?? "");

                //}

            }
            catch (Exception ex)
            {
                // _logService.Error(
                // "{$Class}.{$Method}. Unhandled error." +
                // "\nOrder {$OrderNumber}," +
                // "\nWorksheet {$Worksheet}," +
                // "\nLine {$Line}," +
                // "\nReferenceBase {$ReferenceBase}, " +
                // "\nReference {$Reference}," +
                // "\nColor {$Color}, " +
                // "\nDescription {$Description}," +
                // "\nException: {$Exception}",
                // nameof(PrefSuiteDataService),
                // nameof(InsertPrefSuiteMaterialAsync),
                // material.OrderNumber ?? string.Empty,
                // material.Worksheet ?? string.Empty,
                // material.Line,
                // material.ReferenceBase ?? string.Empty,
                // material.Reference ?? string.Empty,
                // material.Color ?? string.Empty,
                // material.Description ?? string.Empty,
                // ex.Message ?? string.Empty
                //);
                // return new ErrorEntity()
                // {
                //     OrderNumber = material.OrderNumber ?? string.Empty,
                //     Level = ErrorLevel.Error,
                //     Code = ErrorCode.DatabaseWrite_Material,
                //     Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialAsync)}. Unhandled error." +
                //    $"\nOrder {material.OrderNumber ?? string.Empty}," +
                //    $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                //    $"\nLine {material.Line}," +
                //    $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                //    $"\nReference {material.Reference ?? string.Empty}," +
                //    $"\nColor {material.Color ?? string.Empty}, " +
                //    $"\nDescription {material.Description ?? string.Empty}," +
                //    $"\nException: {ex.Message ?? string.Empty}"

                // };
            }

        }
        public async Task InsertPrefSuiteMaterialProfileAsync(MaterialEntity material)
        {

            try

            {
                if (material.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                    material.Weight = weight;
                }


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialProfile]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@PackageQuantity", material.PackageQuantity); //required
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    //    _logService.Verbose("{$Class}.{$Method}. Profile {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                    //nameof(PrefSuiteDataService),
                    //nameof(InsertPrefSuiteMaterialProfileAsync),
                    //material.Reference,
                    //material.Color,
                    //material.Description ?? "");
                }

                if (result == 0)
                {

                    //    _logService.Verbose("{$Class}.{$Method}. Profile {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                    //nameof(PrefSuiteDataService),
                    //nameof(InsertPrefSuiteMaterialProfileAsync),
                    //material.Reference,
                    //material.Color,
                    //material.Description ?? "");

                }


            }

            catch (Exception ex)
            {
                //    _logService.Error(
                //    "{$Class}.{$Method}. Unhandled error." +
                //    "\nOrder {$OrderNumber}," +
                //    "\nWorksheet {$Worksheet}," +
                //    "\nLine {$Line}," +
                //    "\nReferenceBase {$ReferenceBase}, " +
                //    "\nReference {$Reference}," +
                //    "\nColor {$Color}, " +
                //    "\nDescription {$Description}," +
                //    "\nException: {$Exception}",
                //    nameof(PrefSuiteDataService),
                //    nameof(InsertPrefSuiteMaterialProfileAsync),
                //    material.OrderNumber ?? string.Empty,
                //    material.Worksheet ?? string.Empty,
                //    material.Line,
                //    material.ReferenceBase ?? string.Empty,
                //    material.Reference ?? string.Empty,
                //    material.Color ?? string.Empty,
                //    material.Description ?? string.Empty,
                //    ex.Message ?? string.Empty
                //   );
                //    return new ErrorEntity()
                //    {
                //        OrderNumber = material.OrderNumber ?? string.Empty,
                //        Level = ErrorLevel.Error,
                //        Code = ErrorCode.DatabaseWrite_Material,
                //        Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialProfileAsync)}. Unhandled error." +
                //       $"\nOrder {material.OrderNumber ?? string.Empty}," +
                //       $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                //       $"\nLine {material.Line}," +
                //       $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                //       $"\nReference {material.Reference ?? string.Empty}," +
                //       $"\nColor {material.Color ?? string.Empty}, " +
                //       $"\nDescription {material.Description ?? string.Empty}," +
                //       $"\nException: {ex.Message ?? string.Empty}"

                //    };
            }

        }


        public async Task InsertPrefSuiteMaterialMeterAsync(MaterialEntity material)
        {

            try

            {



                if (material.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                    material.Weight = weight;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialMeter]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    //    _logService.Verbose("{$Class}.{$Method}. Meter material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                    //nameof(PrefSuiteDataService),
                    //nameof(InsertPrefSuiteMaterialMeterAsync),
                    //material.Reference,
                    //material.Color,
                    //material.Description ?? "");
                }

                if (result == 0)
                {

                    //    _logService.Verbose("{$Class}.{$Method}. Meter material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                    //nameof(PrefSuiteDataService),
                    //nameof(InsertPrefSuiteMaterialMeterAsync),
                    //material.Reference,
                    //material.Color,
                    //material.Description ?? "");

                }
                // return null;

            }
            catch (Exception ex)
            {
                // _logService.Error(
                // "{$Class}.{$Method}. Unhandled error." +
                // "\nOrder {$OrderNumber}," +
                // "\nWorksheet {$Worksheet}," +
                // "\nLine {$Line}," +
                // "\nReferenceBase {$ReferenceBase}, " +
                // "\nReference {$Reference}," +
                // "\nColor {$Color}, " +
                // "\nDescription {$Description}," +
                // "\nException: {$Exception}",
                // nameof(PrefSuiteDataService),
                // nameof(InsertPrefSuiteMaterialMeterAsync),
                // material.OrderNumber ?? string.Empty,
                // material.Worksheet ?? string.Empty,
                // material.Line,
                // material.ReferenceBase ?? string.Empty,
                // material.Reference ?? string.Empty,
                // material.Color ?? string.Empty,
                // material.Description ?? string.Empty,
                // ex.Message ?? string.Empty
                //);
                // return new ErrorEntity()
                // {
                //     OrderNumber = material.OrderNumber ?? string.Empty,
                //     Level = ErrorLevel.Error,
                //     Code = ErrorCode.DatabaseWrite_Material,
                //     Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialMeterAsync)}. Unhandled error." +
                //    $"\nOrder {material.OrderNumber ?? string.Empty}," +
                //    $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                //    $"\nLine {material.Line}," +
                //    $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                //    $"\nReference {material.Reference ?? string.Empty}," +
                //    $"\nColor {material.Color ?? string.Empty}, " +
                //    $"\nDescription {material.Description ?? string.Empty}," +
                //    $"\nException: {ex.Message ?? string.Empty}"

                //};
            }
            //  return null;
        }
        public async Task InsertPrefSuiteMaterialPieceAsync(MaterialEntity material)
        {

            try
            {

                if (material.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                    material.Weight = weight;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPiece]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());
                if (result > 0)
                {
                    //    _logService.Verbose("{$Class}.{$Method}. Piece material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                    //                     nameof(PrefSuiteDataService),
                    //nameof(InsertPrefSuiteMaterialPieceAsync),
                    //material.Reference,
                    //material.Color,
                    //material.Description ?? "");
                }

                if (result == 0)
                {

                    //    _logService.Verbose("{$Class}.{$Method}. Piece material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                    //        nameof(PrefSuiteDataService),
                    //nameof(InsertPrefSuiteMaterialPieceAsync),
                    //material.Reference,
                    //material.Color,
                    //material.Description ?? "");

                }


            }
            catch (Exception ex)
            {
                // _logService.Error(
                // "{$Class}.{$Method}. Unhandled error." +
                // "\nOrder {$OrderNumber}," +
                // "\nWorksheet {$Worksheet}," +
                // "\nLine {$Line}," +
                // "\nReferenceBase {$ReferenceBase}, " +
                // "\nReference {$Reference}," +
                // "\nColor {$Color}, " +
                // "\nDescription {$Description}," +
                // "\nException: {$Exception}",
                // nameof(PrefSuiteDataService),
                // nameof(InsertPrefSuiteMaterialPieceAsync),
                // material.OrderNumber ?? string.Empty,
                // material.Worksheet ?? string.Empty,
                // material.Line,
                // material.ReferenceBase ?? string.Empty,
                // material.Reference ?? string.Empty,
                // material.Color ?? string.Empty,
                // material.Description ?? string.Empty,
                // ex.Message ?? string.Empty
                //);
                // return new ErrorEntity()
                // {
                //     OrderNumber = material.OrderNumber ?? string.Empty,
                //     Level = ErrorLevel.Error,
                //     Code = ErrorCode.DatabaseWrite_Material,
                //     Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialPieceAsync)}. Unhandled error." +
                //    $"\nOrder {material.OrderNumber ?? string.Empty}," +
                //    $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                //    $"\nLine {material.Line}," +
                //    $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                //    $"\nReference {material.Reference ?? string.Empty}," +
                //    $"\nColor {material.Color ?? string.Empty}, " +
                //    $"\nDescription {material.Description ?? string.Empty}," +
                //    $"\nException: {ex.Message ?? string.Empty}"


                //    };
                //  return null;
            }
        }
        public async Task InsertPrefSuiteMaterialSurfaceAsync(MaterialEntity material)
        {

            try

            {
                if (material.Weight == 0)
                {
                    var weight = await GetTechDesignWeight(material.SourceReference ?? string.Empty);
                    material.Weight = weight;
                }

                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPreSuiteMaterialSurface]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase); //required   
                _ = cmd.Parameters.AddWithValue("@Weight", material.Weight); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", material.MaterialType); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    //    _logService.Verbose("($Class}.{$Method}. Surface material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                    //        nameof(PrefSuiteDataService),
                    //nameof(InsertPrefSuiteMaterialSurfaceAsync),
                    //material.Reference,
                    //material.Color,
                    //material.Description ?? "");
                }

                if (result == 0)
                {

                    //_logService.Verbose("($Class}.{$Method}. Surface material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                    //    nameof(PrefSuiteDataService),
                    //    nameof(InsertPrefSuiteMaterialSurfaceAsync),
                    //    material.Reference,
                    //    material.Color,
                    //    material.Description ?? "");

                }


            }
            catch (Exception ex)
            {
                //    _logService.Error(
                //    "{$Class}.{$Method}. Unhandled error." +
                //    "\nOrder {$OrderNumber}," +
                //    "\nWorksheet {$Worksheet}," +
                //    "\nLine {$Line}," +
                //    "\nReferenceBase {$ReferenceBase}, " +
                //    "\nReference {$Reference}," +
                //    "\nColor {$Color}, " +
                //    "\nDescription {$Description}," +
                //    "\nException: {$Exception}",
                //    nameof(PrefSuiteDataService),
                //    nameof(InsertPrefSuiteMaterialSurfaceAsync),
                //    material.OrderNumber ?? string.Empty,
                //    material.Worksheet ?? string.Empty,
                //    material.Line,
                //    material.ReferenceBase ?? string.Empty,
                //    material.Reference ?? string.Empty,
                //    material.Color ?? string.Empty,
                //    material.Description ?? string.Empty,
                //    ex.Message ?? string.Empty
                //   );
                //    return new ErrorEntity()
                //    {
                //        OrderNumber = material.OrderNumber ?? string.Empty,
                //        Level = ErrorLevel.Error,
                //        Code = ErrorCode.DatabaseWrite_Material,
                //        Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialSurfaceAsync)}. Unhandled error." +
                //       $"\nOrder {material.OrderNumber ?? string.Empty}," +
                //       $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                //       $"\nLine {material.Line}," +
                //       $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                //       $"\nReference {material.Reference ?? string.Empty}," +
                //       $"\nColor {material.Color ?? string.Empty}, " +
                //       $"\nDescription {material.Description ?? string.Empty}," +
                //       $"\nException: {ex.Message ?? string.Empty}"

                //    };
            }



        }

        public async Task InsertPrefSuiteMaterialPurchaseDataAsync(MaterialEntity material)
        {



            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialPurchaseData]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Reference", material.Reference); //required   
                _ = cmd.Parameters.AddWithValue("@Package", material.PackageQuantity); //required
                _ = cmd.Parameters.AddWithValue("@Price", material.Price); //required
                _ = cmd.Parameters.AddWithValue("@Description", material.Description); //required
                _ = cmd.Parameters.AddWithValue("@Color", material.Color); //required
                _ = cmd.Parameters.AddWithValue("@SourceReference", material.SourceReference); //required
                _ = cmd.Parameters.AddWithValue("@SourceColor", material.SourceColor); //required
                _ = cmd.Parameters.AddWithValue("@MaterialType", material.MaterialType); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    // _logService.Verbose("($Class}.{$Method}. Purchase data material {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                    //        nameof(PrefSuiteDataService),
                    //nameof(InsertPrefSuiteMaterialPurchaseDataAsync),
                    //material.Reference,
                    //material.Color,
                    //material.Description ?? "");
                }

                if (result == 0)
                {

                    //_logService.Verbose("($Class}.{$Method}. Purchase data material {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                    //    nameof(PrefSuiteDataService),
                    //    nameof(InsertPrefSuiteMaterialPurchaseDataAsync),
                    //    material.Reference,
                    //    material.Color,
                    //    material.Description ?? "");

                }


            }
            catch (Exception ex)
            {

                // _logService.Error(
                // "{$Class}.{$Method}. Unhandled error." +
                // "\nOrder {$OrderNumber}," +
                // "\nWorksheet {$Worksheet}," +
                // "\nLine {$Line}," +
                // "\nReferenceBase {$ReferenceBase}, " +
                // "\nReference {$Reference}," +
                // "\nColor {$Color}, " +
                // "\nDescription {$Description}," +
                // "\nException: {$Exception}",
                // nameof(PrefSuiteDataService),
                // nameof(InsertPrefSuiteMaterialPurchaseDataAsync),
                // material.OrderNumber ?? string.Empty,
                // material.Worksheet ?? string.Empty,
                // material.Line,
                // material.ReferenceBase ?? string.Empty,
                // material.Reference ?? string.Empty,
                // material.Color ?? string.Empty,
                // material.Description ?? string.Empty,
                // ex.Message ?? string.Empty
                //   );
                //    return new ErrorEntity()
                //    {
                //        OrderNumber = material.OrderNumber ?? string.Empty,
                //        Level = ErrorLevel.Error,
                //        Code = ErrorCode.DatabaseWrite_Material,
                //        Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialPurchaseDataAsync)}. Unhandled error." +
                //       $"\nOrder {material.OrderNumber ?? string.Empty}," +
                //       $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                //       $"\nLine {material.Line}," +
                //       $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                //       $"\nReference {material.Reference ?? string.Empty}," +
                //       $"\nColor {material.Color ?? string.Empty}, " +
                //       $"\nDescription {material.Description ?? string.Empty}," +
                //       $"\nException: {ex.Message ?? string.Empty}"

                //    };
                //}

            }
        }



        public async Task UpdateBCMapping(MaterialEntity material)
        {

            try

            {


                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_UpdateBCMapping]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@ReferenceBase", material.ReferenceBase ?? (object)DBNull.Value); //required   
                _ = cmd.Parameters.AddWithValue("@Reference", material.Reference ?? (object)DBNull.Value); //required   
                _ = cmd.Parameters.AddWithValue("@SourceReference", material.SourceReference ?? (object)DBNull.Value); //required   
                _ = cmd.Parameters.AddWithValue("@SourceColor", material.SourceColor ?? (object)DBNull.Value); //required  
                _ = cmd.Parameters.AddWithValue("@SourceColor1", material.CustomField1 ?? (object)DBNull.Value); //required  
                _ = cmd.Parameters.AddWithValue("@SourceColor2", material.CustomField2 ?? (object)DBNull.Value); //required  

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());

                if (result > 0)
                {
                    //    _logService.Verbose("($Class}.{$Method}. BC Mapping  {$Reference} color {$Color}, {$Description} successfully inserted into PrefSuite DB.",
                    //        nameof(PrefSuiteDataService),
                    //nameof(UpdateBCMapping),
                    //material.Reference,
                    //material.Color,
                    //material.Description ?? "");
                }

                if (result == 0)
                {

                    //_logService.Verbose("($Class}.{$Method}. BC Mapping {$Reference} color {$Color}, {$Description} already exists in PrefSuite DB.",
                    //    nameof(PrefSuiteDataService),
                    //    nameof(UpdateBCMapping),
                    //    material.Reference,
                    //    material.Color,
                    //    material.Description ?? "");

                }

            }
            catch (Exception ex)
            {

                //    _logService.Error(
                //    "{$Class}.{$Method}. Unhandled error." +
                //    "\nOrder {$OrderNumber}," +
                //    "\nWorksheet {$Worksheet}," +
                //    "\nLine {$Line}," +
                //    "\nReferenceBase {$ReferenceBase}, " +
                //    "\nReference {$Reference}," +
                //    "\nColor {$Color}, " +
                //    "\nDescription {$Description}," +
                //    "\nException: {$Exception}",
                //    nameof(PrefSuiteDataService),
                //    nameof(UpdateBCMapping),
                //    material.OrderNumber ?? string.Empty,
                //    material.Worksheet ?? string.Empty,
                //    material.Line,
                //    material.ReferenceBase ?? string.Empty,
                //    material.Reference ?? string.Empty,
                //    material.Color ?? string.Empty,
                //    material.Description ?? string.Empty,
                //    ex.Message ?? string.Empty
                //   );
                //    return new ErrorEntity()
                //    {
                //        OrderNumber = material.OrderNumber ?? string.Empty,
                //        Level = ErrorLevel.Error,
                //        Code = ErrorCode.DatabaseWrite_Material,
                //        Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialSurfaceAsync)}. Unhandled error." +
                //       $"\nOrder {material.OrderNumber ?? string.Empty}," +
                //       $"\nWorksheet {material.Worksheet ?? string.Empty}," +
                //       $"\nLine {material.Line}," +
                //       $"\nReferenceBase {material.ReferenceBase ?? string.Empty}, " +
                //       $"\nReference {material.Reference ?? string.Empty}," +
                //       $"\nColor {material.Color ?? string.Empty}, " +
                //       $"\nDescription {material.Description ?? string.Empty}," +
                //       $"\nException: {ex.Message ?? string.Empty}"

                //    };
            }

        }



        public async Task InsertPrefSuiteMaterialNeedsMasterAsync(string order, int number, int version)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeedsMaster]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Number", number); //required
                _ = cmd.Parameters.AddWithValue("@Version", version); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());


            }
            catch (Exception ex)
            {
                //    _logService.Error(
                //    "{$Class}.{$Method}. Unhandled error." +
                //    "\nOrder {$OrderNumber}," +
                //    "\nSalesDocument {$Number}/{$Version}." +
                //    "\nException {$Exception}.",
                //    nameof(PrefSuiteDataService),
                //    nameof(InsertPrefSuiteMaterialNeedsMasterAsync),
                //    order ?? string.Empty,
                //    number,
                //    version,
                //    ex.Message ?? string.Empty
                //   );
                //    return new ErrorEntity()
                //    {
                //        OrderNumber = order ?? string.Empty,
                //        Level = ErrorLevel.Error,
                //        Code = ErrorCode.DatabaseWrite_Material,
                //        Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialNeedsMasterAsync)}. Unhandled error." +
                //        $"\nOrder {order ?? string.Empty}," +
                //        $"\nSalesDocument {number}/{version}." +
                //        $"\nException: {ex.Message ?? string.Empty}"

                //    };
            }

        }
        public async Task InsertPrefSuiteMaterialNeedsAsync(string order, int number, int version)
        {

            try

            {
                SqlCommand cmd = new()
                {
                    CommandText = "[dbo].[Uniwave_a2p_InsertPrefSuiteMaterialNeeds]",
                    CommandType = CommandType.StoredProcedure
                };
                //=====================================================================================================================
                _ = cmd.Parameters.AddWithValue("@Number", number); //required
                _ = cmd.Parameters.AddWithValue("@Version", version); //required

                //=====================================================================================================================
                int result = await _sqlService.ExecuteNonQueryAsync(cmd.CommandText, cmd.CommandType, cmd.Parameters.Cast<SqlParameter>().ToArray());


            }
            catch (Exception ex)
            {
                //    _logService.Error(
                //    "{$Class}.{$Method}. Unhandled error." +
                //    "\nOrder {$OrderNumber}," +
                //    "\nSalesDocument {$Number}/{$Version}." +
                //    "\nException {$Exception}.",
                //    nameof(PrefSuiteDataService),
                //    nameof(InsertPrefSuiteMaterialNeedsAsync),
                //    order ?? string.Empty,
                //    number,
                //    version,
                //    ex.Message ?? string.Empty
                //   );
                //    return new ErrorEntity()
                //    {
                //        OrderNumber = order ?? string.Empty,
                //        Level = ErrorLevel.Error,
                //        Code = ErrorCode.DatabaseWrite_Material,
                //        Message = $"{nameof(PrefSuiteDataService)}.{nameof(InsertPrefSuiteMaterialNeedsAsync)}. Unhandled error." +
                //        $"\nOrder {order ?? string.Empty}," +
                //        $"\nSalesDocument {number}/{version}." +
                //        $"\nException: {ex.Message ?? string.Empty}"

                //    };
            }

        }


    }
}

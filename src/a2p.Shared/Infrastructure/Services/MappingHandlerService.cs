using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Services;
using a2p.Shared.Core.Interfaces.Services.Other.Mappers;

namespace a2p.Shared.Infrastructure.Services
{
    public class MappingHandlerService : IMappingHandlerService
    {
        private readonly ILogService _logService;

        private readonly IMapperServiceSapa_V2 _mapperSapa_V2;
        private readonly IWriteService _writeService;
        private ProgressValue _progressValue;

        public MappingHandlerService(ILogService logService,
              IWriteService writeService,
              IMapperServiceSapa_V2 mapperSapa_V2)

        {
            _logService = logService;
            _mapperSapa_V2 = mapperSapa_V2;
            _writeService = writeService;
            _progressValue = new ProgressValue();
        }

        public async Task<IEnumerable<A2POrder>> MapDataAsync(IEnumerable<A2POrder> a2POrderList, IProgress<ProgressValue>? progress = null)
        {
            try
            {
                if (a2POrderList.Count() == 0)
                {
                    _logService.Error("Import Service: Order list is null");
                    return a2POrderList;


                }


                int orderCount = 0;
                _progressValue.MaxValue = a2POrderList.Count();
                _progressValue.MinValue = 0;
                _progressValue.Value = 0;
                _progressValue.ProgressTitle = $"Processing Orders...";
                _progressValue.ProgressTask1 = string.Empty;
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;
                progress?.Report(_progressValue);

                foreach (A2POrder a2pOrder in a2POrderList)
                {
                    _progressValue.ProgressTitle = $"Importing Order # {a2pOrder.Order}. Order {orderCount + 1} of {a2POrderList.Count()}";
                    _progressValue.Value = orderCount + 1;
                    _progressValue.ProgressTask1 = $"Processing Files...";
                    progress?.Report(_progressValue);

                    if (a2pOrder == null)
                    {
                        _logService.Error("MS: Order is null");
                        continue;
                    }
                    if (a2pOrder.SalesDocVersion == 0)
                    {
                        _logService.Error("MS: SalesDocVersion is null");
                        continue;
                    }

                    if (a2pOrder.Files == null)
                    {
                        _logService.Error($"Import Service: Files of Order # {a2pOrder.Order} are null!");
                        continue;
                    }


                    //Fort Each File in Order
                    //===================================================================================================================================
                    int fileCount = 0;
                    foreach (A2PFile file in a2pOrder.Files)
                    {

                        _progressValue.ProgressTask1 = $"Importing file {file.FileName}. File {fileCount + 1} of {a2pOrder?.Files.Count}.";
                        _progressValue.ProgressTask2 = $"Processing Worksheets...";
                        progress?.Report(_progressValue);

                        if (a2pOrder == null)
                        {
                            _logService.Error("MS: Error at file ${File} a2pOrder is null", file.FileName);
                            continue;
                        }

                        if (file.Worksheets == null)
                        {
                            _logService.Error($"MS: Worksheets in file {file.FileName} are null!");
                            continue;
                        }



                        //Fort Each File in Order
                        //===================================================================================================================================
                        int worksheetCount = 0;
                        foreach (A2PWorksheet worksheet in file.Worksheets)

                        {
                            _progressValue.ProgressTask2 = $"Processing Worksheet {worksheet.Worksheet}. Worksheet {worksheetCount + 1} of {file.Worksheets.Count}.";
                            _progressValue.ProgressTask3 = $"Processing Rows...";
                            progress?.Report(_progressValue);

                            if (worksheet == null)
                            {
                                _logService.Error("MS: Error Worksheet in file {File} is null", file.FileName);
                                continue;
                            }

                            if (worksheet.RowCount == 0)
                            {
                                _logService.Error("MS: Error in file {File}, worksheet {$Worksheet} row count is 0.", file.FileName, worksheet.Worksheet);
                                continue;
                            }



                            _logService.Debug("MS. Start importing a2pOrder {$Order}, {$WorksheetType}", worksheet.Order ?? "Unknown", worksheet.Type.ToString());


                            if (worksheet.Type.ToString().Contains("v1") && worksheet.Type.ToString().Contains("Item"))
                            {

                            }
                            else if (worksheet.Type.ToString().Contains("v1") && !worksheet.Type.ToString().Contains("Item"))
                            {

                            }

                            //SAPA V2 Items
                            //===================================================================================================================================
                            else if (worksheet.Type.ToString().Contains("v2") && worksheet.Type.ToString().Contains("Item"))
                            {
                                var itemsDTO = await _mapperSapa_V2.MapItemsAsync(worksheet);


                                if (a2pOrder.OverwriteOrder == true)
                                    await _writeService.DeleteItemsAsync(worksheet.Order!);




                                //Fort each mapped items from Worksheet
                                //===================================================================================================================================
                                DateTime dateTime = DateTime.UtcNow;

                                foreach (var itemDTO in itemsDTO)
                                {
                                    try
                                    {
                                        //        await _writeService.DeleteItemsAsync(worksheet.Order!);
                                        var writeResult = await _writeService.InsertItemAsync(itemDTO, a2pOrder.SalesDocNumber, a2pOrder.SalesDocVersion, dateTime);
                                        if (writeResult > 0)
                                        {
                                            _logService.Debug("MS: Material of Order :{$Order }, worksheet {$Worksheet}, Line {$Line}, Item {$Item} - inserted successfully. Inserted record count {$Records}", itemDTO.Order, itemDTO.Worksheet, itemDTO.Line, itemDTO.Item, writeResult);

                                        }
                                        else
                                        {
                                            _logService.Error("MS: Error while inserting itemDTO. Order :{$Order }, worksheet {$Worksheet}, Line {$Line}, Item {$Item} - inserted successfully. Inserted record count {$Records}", itemDTO.Order, itemDTO.Worksheet, itemDTO.Line, itemDTO.Item, writeResult);
                                            a2pOrder.WriteErrors.Add(new A2POrderError
                                            {
                                                Order = a2pOrder.Order,
                                                Level = ErrorLevel.Error,
                                                Code = ErrorCode.MappingService_MapMaterial,
                                                Description = $"Material of Order :{itemDTO.Order}, " +
                                                              $"worksheet {itemDTO.Worksheet}, " +
                                                              $"Line {itemDTO.Line}, " +
                                                              $"Item {itemDTO.Item}" +
                                                              $" - inserted failed. " +
                                                              $"Inserted record count {writeResult}"
                                            });

                                        }
                                    }

                                    catch (Exception ex)
                                    {

                                        _logService.Error("MS TEST1:  Error while inserting itemDTO. Order :{$Order }, worksheet {$Worksheet}, Line {$Line}, Reference {$Reference} - inserted successfully. Exception: {$Exception} ", itemDTO.Order, itemDTO.Worksheet, itemDTO.Line, itemDTO.Item, ex.Message);
                                        _logService.Error(ex, "MS TEST2: Error while inserting itemDTO. Order :{$Order }, worksheet {$Worksheet}, Line {$Line}, Reference {$Reference} - inserted successfully.", itemDTO.Order, itemDTO.Worksheet, itemDTO.Line, itemDTO.Item);

                                        continue;
                                    }

                                }
                            }


                            //SAPA V2 MAterials
                            //===================================================================================================================================
                            else if (worksheet.Type.ToString().Contains("v2") && !worksheet.Type.ToString().Contains("Item"))
                            {
                                var materialsDTO = await _mapperSapa_V2.MapMaterialAsync(worksheet);

                                if (a2pOrder.OverwriteOrder == true)
                                    await _writeService.DeleteMaterialsAsync(worksheet.Order!);

                                //Fort each mapped material from Worksheet
                                //===================================================================================================================================
                                DateTime dateTime = DateTime.UtcNow;

                                foreach (var materialDTO in materialsDTO)
                                {
                                    try
                                    {
                                        //await _writeService.DeleteMaterialsAsync(worksheet.Order);
                                        var writeResult = await _writeService.InsertMaterialAsync(materialDTO, a2pOrder.SalesDocNumber, a2pOrder.SalesDocVersion, dateTime);
                                        _logService.Debug("MS: Material of Order :{$Order }, worksheet {$Worksheet}, Line {$Line}, Reference {$Reference} - inserted successfully. Inserted record count {$Records}", materialDTO.Order, materialDTO.Worksheet, materialDTO.Line, materialDTO.Reference, writeResult);
                                    }
                                    catch (Exception ex)
                                    {

                                        _logService.Error("MS TEST1:  Error while inserting materialDTO. Order :{$Order }, worksheet {$Worksheet}, Line {$Line}, Reference {$Reference} - inserted successfully. Exception: {$Exception} ", materialDTO.Order, materialDTO.Worksheet, materialDTO.Line, materialDTO.Reference, ex.Message);
                                        _logService.Error(ex, "MS TEST2: Error while inserting materialDTO. Order :{$Order }, worksheet {$Worksheet}, Line {$Line}, Reference {$Reference} - inserted successfully.", materialDTO.Order, materialDTO.Worksheet, materialDTO.Line, materialDTO.Reference);

                                        continue;
                                    }

                                }
                            }
                            else if (worksheet.Type.ToString().Contains("Schuco") && worksheet.Type.ToString().Contains("Item"))
                            {

                            }
                            else if (worksheet.Type.ToString().Contains("Schuco") && !worksheet.Type.ToString().Contains("Item"))
                            {

                            }

                            else
                            {
                                _logService.Error("MS: Error  in file {File}, worksheet {$Worksheet} type is Unknown", file.FileName, worksheet.Worksheet);
                                continue;
                            }

                            _logService.Debug("Import Service. Finish importing a2pOrder {$Order}, {WorksheetType}", worksheet.Order ?? "Unknown", worksheet.Type.ToString());




                            if (file.Worksheets == null)
                            {
                                _logService.Error($"Import Service: Worksheet in file {file.FileName} is null!");
                                continue;
                            }
                            fileCount++;
                        }
                        orderCount++;
                    }

                }


                return a2POrderList;
            }

            catch (Exception ex)
            {
                _logService.Error(ex, "MS: Unhandled Error while importing data", ex.Message);
                throw;

            }

        }

    }
}


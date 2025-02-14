using a2p.Shared.Core.DTO;
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
        private IProgress<ProgressValue> _progress;
        public MappingHandlerService(ILogService logService,
              IWriteService writeService,
              IMapperServiceSapa_V2 mapperSapa_V2)

        {
            _logService = logService;
            _mapperSapa_V2 = mapperSapa_V2;
            _writeService = writeService;
            _progressValue = new ProgressValue();

        }

        public async Task<IEnumerable<A2POrder>> MapDataAsync(IEnumerable<A2POrder> a2pOrderList, Progress<ProgressValue>? progress = null)
        {
            _progress = progress ?? new Progress<ProgressValue>();

            try
            {
                if (!a2pOrderList.Any())
                {
                    _logService.Warning("Mapping handler service: Error mapping data. Order list count is 0.!");
                    return a2pOrderList ?? [];

                }
                int orderCount = 0;

                int progressValueCount = 0;
                _progressValue.MaxValue = a2pOrderList.Count() * 2;
                _progressValue.MinValue = 0;
                _progressValue.Value = 0;
                _progressValue.ProgressTitle = $"Processing Orders...";
                _progressValue.ProgressTask1 = string.Empty;
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);


                foreach (A2POrder a2pOrder in a2pOrderList)
                {


                    progressValueCount += 1;

                    _progressValue.ProgressTitle = $"Importing Order # {a2pOrder.Order}. Order {orderCount + 1} of {a2pOrderList.Count()}";
                    _progressValue.Value = progressValueCount;
                    _progressValue.ProgressTask1 = $"Processing Files...";
                    _progress?.Report(_progressValue);



                    if (a2pOrder.Files == null || !a2pOrder.Files.Any())
                    {
                        _logService.Error("Mapping handler service: Error mapping data.Order {$Order}, files not found! ", a2pOrder.Order);
                        continue;
                    }

                    if (a2pOrder.OverwriteOrder == true)
                    {
                        _progressValue.ProgressTask2 = $"Deleting existing data... ";
                        _progress?.Report(_progressValue);

                        _ = await _writeService.DeleteItemsAsync(a2pOrder.Order);
                        _ = await _writeService.DeleteMaterialsAsync(a2pOrder.Order);
                        a2pOrder.OverwriteOrder = false;
                        _progressValue.ProgressTask2 = $"Data deleted...  ";
                        _progress?.Report(_progressValue);
                    }



                    //Fort Each File in Order
                    //===================================================================================================================================
                    int fileCount = 0;
                    foreach (A2PFile a2pFile in a2pOrder.Files)
                    {


                        _progressValue.ProgressTask1 = $"Importing File {a2pFile.FileName}. File {fileCount + 1} of {a2pOrder?.Files.Count}.";
                        _progressValue.ProgressTask2 = $"Processing Worksheets...";
                        _progress?.Report(_progressValue);


                        if (a2pFile.Worksheets == null || !a2pFile.Worksheets.Any())
                        {
                            _logService.Error("Mapping handler service: Error mapping data.Order {$Order}, file {$File} worksheets not found! ", a2pOrder!.Order, a2pFile.FileName);
                            continue;
                        }

                        //Fort Each Worksheet in Order
                        //===================================================================================================================================
                        int worksheetCount = 0;
                        foreach (A2PWorksheet a2pWorksheet in a2pFile.Worksheets)

                        {
                            _progressValue.ProgressTask2 = $"Processing Worksheet {a2pWorksheet.Worksheet}. Worksheet {worksheetCount + 1} of {a2pFile.Worksheets!.Count}.";
                            _progressValue.ProgressTask3 = $"Processing Rows...";
                            _progress?.Report(_progressValue);

                            if (a2pWorksheet.RowCount == 0)
                            {
                                _logService.Error("Mapping handler service: Error mapping data. Order {$Order}, file ${File}, worksheet {$Worksheet} has no rows.", a2pOrder!.Order, a2pFile!.FileName, a2pWorksheet.Worksheet);
                                continue;
                            }

                            _logService.Debug("Mapping handler service: Start mapping order {$Order}, " +
                                                                                     "file ${File}, " +
                                                                                     "worksheet {$Worksheet} rows. " +
                                                                                     "Rows count {Rows} ",
                                                                                     a2pOrder!.Order,
                                                                                     a2pFile.FileName,
                                                                                     a2pWorksheet.Worksheet,
                                                                                     a2pWorksheet.RowCount);


                            DateTime dateTime = DateTime.UtcNow;

                            //SAPA V2 Items
                            //===================================================================================================================================
                            if (a2pWorksheet.Type == WorksheetType.Items_Sapa_v2)
                            {


                                //Map Items
                                //===================================================================================================================================
                                List<ItemDTO> itemsDTO = await _mapperSapa_V2.MapItemsAsync(a2pWorksheet);


                                //Write Items
                                //===================================================================================================================================
                                foreach (Core.DTO.ItemDTO itemDTO in itemsDTO)
                                {
                                    try
                                    {
                                        int writeResult = await _writeService.InsertItemAsync(itemDTO, a2pOrder.SalesDocNumber, a2pOrder.SalesDocVersion, dateTime);
                                        if (writeResult > 0)
                                        {
                                            _logService.Debug("Mapping handler service: Material of Order :{$Order}, worksheet {$Worksheet}, line {$Line}, item {$Item} - inserted successfully. Inserted record count {$Records}", itemDTO.Order, itemDTO.Worksheet, itemDTO.Line, itemDTO.Item ?? "Unknown", writeResult);

                                        }
                                        else
                                        {
                                            _logService.Error("Mapping handler service: Error while inserting itemDTO. Order :{$Order}, worksheet {$Worksheet}, line {$Line}, item {$Item} - insert failed.", itemDTO.Order, itemDTO.Worksheet, itemDTO.Line, itemDTO.Item ?? "Unknown");
                                            a2pOrder.WriteErrors.Add(new A2POrderError
                                            {
                                                Order = a2pOrder.Order,
                                                Level = ErrorLevel.Error,
                                                Code = ErrorCode.WriteService_ItemWrite,
                                                Description = $"Order :{itemDTO.Order}, " +
                                                              $"worksheet :{itemDTO.Worksheet}, " +
                                                              $"line {itemDTO.Line}, " +
                                                              $"item {itemDTO.Item ?? "Unknown"}" +
                                                              $" insert failed. "
                                            });

                                        }
                                    }

                                    catch (Exception ex)
                                    {

                                        _logService.Error("Mapping handler service: Error while inserting itemDTO. Order :{$Order}, worksheet {$Worksheet}, line {$Line}, item {$Item} inserted failed. Exception: {$Exception} ", itemDTO.Order, itemDTO.Worksheet, itemDTO.Line, itemDTO.Item ?? "Unknown", ex.Message);
                                        continue;
                                    }

                                }
                            }


                            //SAPA V2 Materials
                            //===================================================================================================================================
                            if (a2pWorksheet.Type is WorksheetType.Glasses_Sapa_v2 or WorksheetType.Materials_Sapa_v2 or WorksheetType.Panels_Sapa_v2)
                            {

                                //Map Materials
                                List<MaterialDTO> materialsDTO = await _mapperSapa_V2.MapMaterialAsync(a2pWorksheet);

                                //Write Materials
                                //===================================================================================================================================
                                foreach (Core.DTO.MaterialDTO materialDTO in materialsDTO)
                                {
                                    try
                                    {
                                        int writeResult = await _writeService.InsertMaterialListAsync(materialDTO, a2pOrder.SalesDocNumber, a2pOrder.SalesDocVersion, dateTime);
                                        if (writeResult > 0)
                                        {
                                            _logService.Debug("Mapping handler service: Material of Order :{$Order}, worksheet {$Worksheet}, line {$Line}, material {$material} - inserted successfully. Inserted record count {$Records}", materialDTO.Order, materialDTO.Worksheet, materialDTO, materialDTO.Reference ?? "Unknown", writeResult);
                                        }
                                        else
                                        {
                                            _logService.Error("Mapping handler service: Error while inserting materialDTO. Order :{$Order}, worksheet {$Worksheet}, line {$Line}, material {$material} - insert failed.", materialDTO.Order, materialDTO.Worksheet, materialDTO.Line, materialDTO.Reference ?? "Unknown");
                                            a2pOrder.WriteErrors.Add(new A2POrderError
                                            {
                                                Order = a2pOrder.Order,
                                                Level = ErrorLevel.Error,
                                                Code = ErrorCode.WriteService_MaterialWrite,
                                                Description = $"Order :{materialDTO.Order}, Worksheet {materialDTO.Worksheet}, line {materialDTO.Line}, material {materialDTO.Reference ?? "Unknown"} insert failed."

                                            });
                                        }
                                    }

                                    catch (Exception ex)
                                    {

                                        _logService.Error("Mapping handler service: Error while inserting itemDTO. Order :{$Order}, worksheet {$Worksheet}, line {$Line}, item {$Item} inserted failed. Exception: {$Exception} ", materialDTO.Order, materialDTO.Worksheet, materialDTO.Line, materialDTO.Reference ?? "Unknown", ex.Message);


                                        continue;
                                    }

                                }
                            }
                            worksheetCount++;
                        }
                        fileCount++;
                    }
                    orderCount++;
                    progressValueCount += 1;
                }


                return a2pOrderList.ToList();
            }

            catch (Exception ex)
            {
                _logService.Error("Mapping handler service: Unhandled Error while mapping data. Exception: ${Exception} ", ex.Message);
                throw;

            }

        }


    }
}


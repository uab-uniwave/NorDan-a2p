using a2p.Shared.Core.Entities.Models;
using a2p.Shared.Core.Enums;
using a2p.Shared.Core.Interfaces.Mappers;
using a2p.Shared.Core.Interfaces.Services.Read;
using a2p.Shared.Core.Utils;

namespace a2p.Shared.Infrastructure.Services.Read
{
    public class ReadService : IReadService
    {
        private readonly ILogService _logger;
        private readonly IGlassMapper _glassMapper;
        private readonly IPanelMapper _panelMapper;
        private readonly IItemMapper _positionMapper;
        private readonly IMaterialMapper _materialMapper;
        private readonly IReadSapa_v1 _readSapa1;
        private readonly IReadSapa_v2 _readSapa2;
        private readonly IReadSchuco _readSchuco;
        private ProgressValue _progressValue;

        public ReadService(ILogService logger,
              IReadSapa_v1 readSapa1,
              IReadSapa_v2 readSapa22,
              IReadSchuco readSchuco,
              IItemMapper positionMapper,
              IMaterialMapper materialMapper,
              IGlassMapper glassMapper,
              IPanelMapper panelMapper)
        {
            _logger = logger;
            _readSapa1 = readSapa1;
            _readSapa2 = readSapa22;
            _readSchuco = readSchuco;
            _positionMapper = positionMapper;
            _materialMapper = materialMapper;
            _glassMapper = glassMapper;
            _panelMapper = panelMapper;
            _progressValue = new ProgressValue();
        }

        public async Task ReadDataAsync(IEnumerable<A2POrder> orderList, IProgress<ProgressValue>? progress = null)
        {
            try
            {
                if (orderList.Count() == 0)
                {
                    _logger.Error("Import Service: Order list is null");
                    throw new ArgumentNullException(nameof(orderList));
                }


                int orderCount = 0;
                _progressValue.MaxValue = orderList.Count();
                _progressValue.MinValue = 0;
                _progressValue.Value = 0;
                _progressValue.ProgressTitle = $"Processing Orders...";
                _progressValue.ProgressTask1 = string.Empty;
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;
                progress?.Report(_progressValue);

                foreach (A2POrder? order in orderList)
                {
                    _progressValue.ProgressTitle = $"Importin Order # {order.Number}. Order {orderCount + 1} of {orderList.Count()}";
                    _progressValue.Value = orderCount + 1;
                    _progressValue.ProgressTask1 = $"Processing Files...";
                    progress?.Report(_progressValue);

                    if (order == null)
                    {
                        _logger.Error("Import Service: Order is null");
                        continue;
                    }

                    if (order.Files == null)
                    {
                        _logger.Error($"Import Service: Files of Order # {order.Number} are null!");
                        continue;
                    }
                    int fileCount = 0;
                    foreach (A2PFile file in order.Files)
                    {

                        _progressValue.ProgressTask1 = $"Importing file {file.Name}. File {fileCount + 1} of {order?.Files.Count}.";
                        _progressValue.ProgressTask2 = $"Processing Worksheets...";
                        progress?.Report(_progressValue);
                        Task.Delay(2000).Wait();

                        if (order == null)
                        {
                            _logger.Error($"Import Service:{order?.Number} file is null");
                            continue;
                        }


                        if (file.FileWorksheets == null)
                        {
                            _logger.Error($"Import Service: Worksheets in file {file.Name} are null!");
                            continue;
                        }

                        int worksheetCount = 0;
                        foreach (A2PWorksheet worksheet in file.FileWorksheets)

                        {
                            _progressValue.ProgressTask2 = $"Importing Worksheet {worksheet.Name}. Worksheet {worksheetCount + 1} of {file.FileWorksheets.Count}.";
                            _progressValue.ProgressTask3 = $"Processing Rows...";
                            progress?.Report(_progressValue);

                            //_sql


                            if (worksheet == null)
                            {
                                _logger.Error("IS: Error Importing Data. Worksheet is null Import Service worksheet is null"!);
                                continue;
                            }


                            _logger.Debug("Import Service. Start importing order {$Order}, {$WorksheetType}", worksheet.Order ?? "Unknown", worksheet.WorksheetType.ToString());
                            switch (worksheet.WorksheetType)
                            {
                                case WorksheetType.Items_Sapa_v1:
                                    _ = await _readSapa1.ImportPositionsAsync(await _positionMapper.MapToPositionDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Items_Sapa_v2:
                                    _ = await _readSapa2.ImportItemsAsync(await _positionMapper.MapToPositionDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Items_Schuco:
                                    _ = await _readSchuco.ReadPositionsAsync(await _positionMapper.MapToPositionDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Materials_Sapa_v1:
                                    _ = await _readSapa1.ImportMaterialsAsync(await _materialMapper.MapToMaterialDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Materials_Sapa_v2:
                                    _ = await _readSapa2.ImportMaterialsAsync(await _materialMapper.MapToMaterialDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Materials_Schuco:
                                    _ = await _readSchuco.ReadMaterialsAsync(await _materialMapper.MapToMaterialDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Glasses_Sapa_v1:
                                    _ = await _readSapa1.ImportGlassesAsync(await _glassMapper.MapToGlassDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Glasses_Sapa_v2:
                                    int? result = await _readSapa2.ImportGlassesAsync(await _glassMapper.MapToGlassDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Glasses_Schuco:
                                    _ = await _readSchuco.ReadGlassesAsync(await _glassMapper.MapToGlassDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Panels_Sapa_v1:
                                    _ = await _readSapa1.ImportPanelsAsync(await _panelMapper.MapToPanelDTOAsync(worksheet));
                                    break;
                                case WorksheetType.Panels_Sapa_v2:
                                    _ = await _readSapa2.ImportPanelsAsync(await _panelMapper.MapToPanelDTOAsync(worksheet));
                                    break;
                                default:
                                    _logger.Error("Import Service. Worksheet type or/and vendor of order {$Order} unknown", worksheet.Order ?? "Unknown");
                                    break;
                            }
                            string worksheetType = worksheet.WorksheetType.ToString();

                            _logger.Debug("Import Service. Finish importing order {$Order}, {WorksheetType}", worksheet.Order ?? "Unknown", worksheetType.ToString());
                            worksheetCount++;
                        }



                        if (file.FileWorksheets == null)
                        {
                            _logger.Error($"Import Service: Worksheet in file {file.Name} is null!");
                            continue;
                        }
                        fileCount++;
                    }
                    orderCount++;
                }

            }

            catch (Exception ex)
            {
                _logger.Error(ex, "Import Service. Unhandled Error while importing data");
                throw;

            }

        }

    }
}


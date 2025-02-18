using a2p.Shared.Application.Interfaces;
using a2p.Shared.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Application.Services
{
    public class ReadService : IReadService
    {
        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private readonly IExcelReadService _excelReadService;
        private readonly IMapperSapaV1 _mapperSapaV1;
        private readonly IMapperSapaV2 _mapperSapaV2;
        private readonly IMapperSchuco _mapperSchuco;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue> _progress;
        public ReadService(ILogService logService,
                           IFileService fileService,
                           IMapperSapaV1 mapperSapaV1,
                           IMapperSapaV2 mapperSapaV2,
                           IMapperSchuco mapperSchuco,
                           IExcelReadService excelReadService)
        {
            _logService = logService;
            _fileService = fileService;
            _mapperSapaV1 = mapperSapaV1;
            _mapperSapaV2 = mapperSapaV2;
            _mapperSchuco = mapperSchuco;
            _excelReadService = excelReadService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
        }

        public async Task<IEnumerable<A2POrder>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {
            _progressValue = progressValue;
            _progress = progress ?? new Progress<ProgressValue>();
            IEnumerable<A2POrder> ordersEnumerable = await _fileService.GetOrdersAsync(_progressValue, _progress);

            List<A2POrder> orders = ordersEnumerable.ToList();

            for (int k = 0; k < orders.Count; k++)
            {
                A2POrder order = orders[k];
                for (int i = 0; i < order.Files.Count; i++)
                {
                    A2PFile file = order.Files[i];
                    for (int j = 0; j < file.Worksheets.Count; j++)
                    {
                        _ = file.Worksheets[j];
                        try
                        {
                            if (order.SourceAppType == SourceAppType.SapaV1 && file.IsOrderItemsFile)
                            {
                                orders[k] = await _mapperSapaV1.MapItemsAsync(order, _progressValue, _progress);
                            }
                            else if (order.SourceAppType == SourceAppType.SapaV1 && !file.IsOrderItemsFile)
                            {
                                orders[k] = await _mapperSapaV2.MapMaterialsAsync(order, _progressValue, _progress);
                            }
                            else if (order.SourceAppType == SourceAppType.SapaV2 && file.IsOrderItemsFile)
                            {
                                orders[k] = await _mapperSapaV1.MapItemsAsync(order, _progressValue, _progress);
                            }
                            else if (order.SourceAppType == SourceAppType.SapaV2 && !file.IsOrderItemsFile)
                            {
                                orders[k] = await _mapperSapaV2.MapMaterialsAsync(order, _progressValue, _progress);
                            }
                            else if (order.SourceAppType == SourceAppType.Schuco && !file.IsOrderItemsFile)
                            {
                                if (order.SourceAppType == SourceAppType.SapaV1 && file.IsOrderItemsFile)
                                {
                                    orders[k] = await _mapperSapaV1.MapItemsAsync(order, _progressValue, _progress);
                                }
                                else if (order.SourceAppType == SourceAppType.SapaV1 && !file.IsOrderItemsFile)
                                {
                                    orders[k] = await _mapperSapaV2.MapMaterialsAsync(order, _progressValue, _progress);
                                }
                                else if (order.SourceAppType == SourceAppType.SapaV2 && file.IsOrderItemsFile)
                                {
                                    orders[k] = await _mapperSapaV1.MapItemsAsync(order, _progressValue, _progress);
                                }
                                else if (order.SourceAppType == SourceAppType.SapaV2 && !file.IsOrderItemsFile)
                                {
                                    orders[k] = await _mapperSapaV2.MapMaterialsAsync(order, _progressValue, _progress);
                                }
                                else if (order.SourceAppType == SourceAppType.Schuco && !file.IsOrderItemsFile)
                                {
                                    orders[k] = await _mapperSapaV2.MapItemsAsync(order, _progressValue, _progress);
                                }
                                else if (order.SourceAppType == SourceAppType.Schuco && !file.IsOrderItemsFile)
                                {
                                    orders[k] = await _mapperSchuco.MapMaterialsAsync(order, _progressValue, _progress);
                                }
                                orders[k] = await _mapperSapaV2.MapItemsAsync(order, _progressValue, _progress);
                            }
                            else if (order.SourceAppType == SourceAppType.Schuco && !file.IsOrderItemsFile)
                            {
                                orders[k] = await _mapperSchuco.MapMaterialsAsync(order, _progressValue, _progress);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logService.Error(ex.Message);
                        }
                    }
                }
            }
            return orders;
        }


    }
}
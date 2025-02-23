using a2p.Shared.Application.Interfaces;
using a2p.Shared.Application.Services.Domain.Entities;
using a2p.Shared.Application.Services.Domain.Enums;
using a2p.Shared.Domain.Entities;
using a2p.Shared.Domain.Enums;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Infrastructure.Services
{
    public class OrderReadProcessor : IOrderReadProcessor
    {
        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private readonly IMapperSapaV1 _mapperSapaV1;
        private readonly IMapperSapaV2 _mapperSapaV2;
        private readonly IMapperSchuco _mapperSchuco;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue> _progress;
        public OrderReadProcessor(ILogService logService,
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
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();
        }

        public async Task<List<A2POrder>> ReadAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            try
            {

                _progressValue = progressValue;
                _progress = progress ?? new Progress<ProgressValue>();

                _progressValue.ProgressTitle = "Loading Orders Files... ";
                _progressValue.ProgressTask1 = $"Reading Orders ...";
                _progressValue.ProgressTask2 = string.Empty;
                _progressValue.ProgressTask3 = string.Empty;

                List<A2POrder> a2pOrders = await _fileService.GetOrdersAsync(_progressValue, _progress);

                _progressValue.ProgressTask3 = string.Empty;
                _progressValue.MinValue = 0;
                _progressValue.MaxValue = a2pOrders.Count * 2;
                _progressValue.Value = 0;
                _progress?.Report(_progressValue);

                int ordersCounter = 0;
                int progressBarValue = 0;
                for (int i = 0; i < a2pOrders.Count; i++)

                {
                    _ = a2pOrders[i];
                    try
                    {

                        ordersCounter++;
                        progressBarValue++;

                        _progressValue.ProgressTitle = $"Reading Order # {a2pOrders[i].Order}. {ordersCounter} of {a2pOrders.Count}";
                        _progressValue.Value = progressBarValue;
                        _progress?.Report(_progressValue);

                    }
                    catch (Exception ex)
                    {
                        _logService.Error("Error reading order {a2pOrders[i].Order}. Exception {$Exception}", a2pOrders[i].Order, ex.Message);
                        a2pOrders[i].ReadErrors.Add(new()
                        {
                            Order = a2pOrders[i].Order,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.MappingService_MapOrder,
                            Message = $"Error reading order {a2pOrders[i].Order}. Exception {ex.Message}"
                        });
                    }

                    finally
                    {
                        progressBarValue++;
                        _progressValue.Value = progressBarValue;
                        _progress?.Report(_progressValue);
                    }

                }
                return a2pOrders;

            }
            catch (Exception ex)
            {
                _logService.Error(ex, "Error reading a2pOrders");
                return [];

            }

        }

    }
}
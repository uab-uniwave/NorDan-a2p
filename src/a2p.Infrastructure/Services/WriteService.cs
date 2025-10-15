// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.Interfaces;
using a2p.Domain.Interfaces;


namespace a2p.Infrastructure.Services
{
    public class WriteService : IWriteService
    {

        private readonly ILogService _logService;
        private readonly Application.Interfaces.IExcelService _fileService;
        private readonly IPrefSuiteDataService _prefSuiteDataService;
        private readonly IOrderRepository _orderRepository;
        private readonly IMaterialRepository _materialRepository;
        private readonly IItemRepository _itemRepository;

        private readonly IExcelService _excelService;
        private readonly IPrefSuiteService _prefSuiteService;
        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;
        public WriteService(ILogService logService, IPrefSuiteService prefSuiteService, Application.Interfaces.IFileService fileService, IExcelService excelService, IPrefSuiteDataService prefSuiteDataServic, IOrderRepository orderRepository, IPrefSuiteDataService prefSuiteDataService,
        IMaterialRepository materialRepository,
                           IItemRepository itemRepository)
        {

            _orderRepository = orderRepository;
            _prefSuiteDataService = prefSuiteDataService;
            _fileService = fileService;
            _excelService = excelService;
            _logService = logService;
            _prefSuiteService = prefSuiteService;
            _orderRepository = orderRepository;
            _materialRepository = materialRepository;
            _itemRepository = itemRepository;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }

        public async Task WriteAsync(ExcelOrderDto order, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            _progressValue = progressValue != null ? progressValue : _progressValue;
            _progress = progress != null ? progress : _progress;

            try
            {


                _progressValue.CurrentValue = _progressValue.CurrentValue + 30;   //30pts. x 1 per Orde               
                _progressValue.ProgressTask2 = "Deleting any existsing data import pending orders.....";
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

                //ErrorEntity? deleteError = await _prefSuiteDataService.DeleteSalesDocumentDataAsync(order.Number, order.Version, order.DeleteExistsing);
                //if (deleteError != null)
                //{
                //    order.Errors.Add(deleteError);
                //}




                await _prefSuiteService.InsertItemsAsync(order, _progressValue, _progress);
                //                var ordersPref = Result.Item1;
                //_progressValue = Result.Item2;


                //if (ordersPref != null)
                //{
                //    order = ordersPref;
                //}

                for (int i = 0; i < order.Items.Count; i++)
                {
                    try
                    {
                        _progressValue.CurrentValue = _progressValue.CurrentValue + 10;  //10pts. x 1 per ItemName
                        _progressValue.ProgressTask2 = $"Inserting items {i + 1} of {order.Items.Count} into db...";
                        _progressValue.ProgressTask3 = $"ItemName # {order.Items[i].ItemName}";

                        _progress?.Report(_progressValue);
                        //ErrorEntity? error = await _orderRepository.InsertOrderItemAsync(order.ItemsDto[i], order.Number, order.Version, order.ItemsDto[i].Id);
                        //if (error != null)
                        //{
                        //    order.Errors.Add(error);
                        //}


                    }
                    catch (Exception ex)
                    {

                        // _logService.Error("{$Class}.{$Method}. Unhandled error in for loop ." +
                        //    "\nUnhandled error: writing order {$OrderNumber}." +
                        //    "\nException {$Exception}",
                        //    nameof(WriteService),
                        //    nameof(WriteAsync),
                        //    order.OrderNumber,
                        //    ex.Message);

                        //order.Errors.Add(new ErrorEntity
                        //{
                        //    OrderNumber = order.OrderNumber,
                        //    Level = ErrorLevel.Error,
                        //    Code = ErrorCode.ERPWrite_Order,
                        //    Message = $"{nameof(WriteService)}.{nameof(WriteAsync)}." +
                        //    $"\nUnhandled error: writing order {order.OrderNumber}." +
                        //    $"\nException {ex.Message}",

                        //});

                    }

                }

                for (int i = 0; i < order.Materials.Count; i++)
                {


                    _progressValue.CurrentValue = _progressValue.CurrentValue + 1;//1pts. x 1 per material
                    _progressValue.ProgressTask2 = $"Inserting materials {i + 1} of {order.Materials.Count} into PrefSuite DB...";
                    _progressValue.ProgressTask3 = $"Material # {order.Materials[i].ReferenceBase} {order.Materials[i].Color}.";
                    _progress?.Report(_progressValue);
                    // }

                    // Insert MaterialDto
                    //=================================================================
                    ////ErrorEntity? ErrorMaterialDTO = await _orderRepository.InsertOrderMaterialAsync(order.Materials[i], order.Number, order.Version);
                    //if (ErrorMaterialDTO != null)
                    //{
                    //   // order.Errors.Add(ErrorMaterialDTO);
                    //    continue;
                    //}


                    // Insert PrefSuite Material
                    //=================================================================
                    //ErrorEntity? errorPrefColor = await _prefSuiteDataService.InsertPrefSuiteColorAsync(order.Materials[i]);
                    //if (errorPrefColor != null)
                    //{
                    //  //  order.Errors.Add(errorPrefColor);
                    //    continue;
                    //}


                    // Insert PrefSuite Material Configuration
                    //=================================================================
                    //int GetColorConfiguration = await _prefSuiteDataService.GetPrefSuiteColorConfigurationAsync(order.Materials[i].Color);
                    //ErrorEntity? errorColorConfiguration = await _prefSuiteDataService.InsertPrefSuiteColorConfigurationAsync(order.Materials[i]);
                    //if (errorColorConfiguration != null)
                    //{
                    // //   order.Errors.Add(errorColorConfiguration);
                    //    continue;
                    //}


                    // Insert PrefSuite Material Base
                    //=================================================================
                    //ErrorEntity? errorPrefMaterialBase = await _prefSuiteDataService.InsertPrefSuiteMaterialBaseAsync(order.Materials[i]);
                    //if (errorPrefMaterialBase != null)
                    //{
                    //   // order.Errors.Add(errorPrefMaterialBase);
                    //    continue;
                    //}

                    // Insert PrefSuite Material 
                    //=================================================================
                    //ErrorEntity? errorPrefMaterial = await _prefSuiteDataService.InsertPrefSuiteMaterialAsync(order.Materials[i]);
                    //if (errorPrefMaterial != null)
                    //{
                    //    order.Errors.Add(errorPrefMaterial);
                    //    continue;
                    //}

                    // Insert PrefSuite material Profile 
                    //=================================================================
                    //if (order.Materials[i].MaterialType == MaterialType.Profiles)
                    //{
                    //    //ErrorEntity? errorPrefProfile = await _prefSuiteDataService.InsertPrefSuiteMaterialProfileAsync(order.Materials[i]);
                    //    //if (errorPrefProfile != null)
                    //    //{
                    //    //  //  order.Errors.Add(errorPrefProfile);
                    //    //    continue;
                    //    //}
                    //}

                    //// Insert PrefSuite material Gaskets
                    ////=================================================================
                    //if (order.Materials[i].MaterialType == MaterialType.Gaskets)
                    //{
                    //    //ErrorEntity? errorPrefMeter = await _prefSuiteDataService.InsertPrefSuiteMaterialMeterAsync(order.Materials[i]);
                    //    //if (errorPrefMeter != null)
                    //    //{
                    //    // //   order.Errors.Add(errorPrefMeter);
                    //    //    continue;
                    //    //}

                    //}


                    //// Insert PrefSuite material Pieces
                    ////=================================================================
                    //if (order.Materials[i].MaterialType == MaterialType.Piece)
                    //{
                    //    ErrorEntity? errorPrefPiece = await _prefSuiteDataService.InsertPrefSuiteMaterialPieceAsync(order.Materials[i]);
                    //    if (errorPrefPiece != null)
                    //    {
                    //    //    order.Errors.Add(errorPrefPiece);
                    //        continue;
                    //    }
                    //}

                    //// Insert PrefSuite material Panels
                    ////===============================================================
                    //if (order.Materials[i].MaterialType == MaterialType.Panels)
                    //{

                    //    ErrorEntity? errorPrefSurface = await _prefSuiteDataService.InsertPrefSuiteMaterialSurfaceAsync(order.Materials[i]);
                    //    if (errorPrefSurface != null)
                    //    {
                    //     //   order.Errors.Add(errorPrefSurface);
                    //        continue;
                    //    }

                    //}


                    //if (order.Materials[i].MaterialType == MaterialType.Glasses)
                    //{

                    //    //ErrorEntity? errorPrefSurface = await _prefSuiteDataService.InsertPrefSuiteMaterialSurfaceAsync(order.Materials[i]);
                    //    //if (errorPrefSurface != null)
                    //    //{
                    //    //  /  order.Errors.Add(errorPrefSurface);
                    //    //    continue;
                    //    //}

                    //}


                    //// Update BC Mapping
                    ////=================================================================
                    //if (order.Materials[i].MaterialType != MaterialType.Glasses)
                    //{
                    //    //ErrorEntity? errorUpdateBC = await _prefSuiteDataService.UpdateBCMapping(order.Materials[i]);
                    //    //if (errorUpdateBC != null)
                    //    //{
                    //    // //   order.Errors.Add(errorUpdateBC);
                    //    //    continue;
                    //    //}

                    //}

                    //// Insert PrefSuite material Purchase Data
                    //if (!string.IsNullOrEmpty(order.Materials[i].Reference))
                    //{
                    //    //ErrorEntity? errorPurchaseData = await _prefSuiteDataService.InsertPrefSuiteMaterialPurchaseDataAsync(order.Materials[i]);
                    //    //if (errorPurchaseData != null)
                    //    //{
                    //    // //   order.Errors.Add(errorPurchaseData);
                    //    //    continue;
                    //    //}
                    //}

                    //}

                    //}
                    //catch (Exception ex)
                    //        {
                    //    //_logService.Error(
                    //"{$Class}.{$Method}. Unhandled error." +
                    //"\nOrder {$OrderNumber}," +
                    //"\nWorksheet {$Worksheet}," +
                    //"\nLine {$Line}," +
                    //"\nReferenceBase {$ReferenceBase}, " +
                    //"\nReference {$Reference}," +
                    //"\nColor {$Color}, " +
                    //"\nColor {$ColorDescription}, " +
                    //"\nDescription {$Description}," +
                    //"\nException: {$Exception}",
                    //nameof(WriteService),
                    //nameof(WriteAsync),
                    //order.Materials[i].OrderNumber ?? string.Empty,
                    //order.Materials[i].Worksheet ?? string.Empty,
                    //order.Materials[i].Line,
                    //order.Materials[i].ReferenceBase ?? string.Empty,
                    //order.Materials[i].Reference ?? string.Empty,
                    //order.Materials[i].Color ?? string.Empty,
                    // order.Materials[i].ColorDescription ?? string.Empty,
                    //order.Materials[i].Description ?? string.Empty,
                    //   //ex.Message ?? string.Empty
                    // );
                    //// order.Errors.Add(new ErrorEntity()
                    //  {
                    //      OrderNumber = order.Materials[i].OrderNumber ?? string.Empty,
                    //      Level = ErrorLevel.Error,
                    //      Code = ErrorCode.DatabaseWrite_Material,
                    //      Message = $"{nameof(WriteService)}.{nameof(WriteAsync)}. Unhandled error." +
                    //     $"\nOrder {order.Materials[i].OrderNumber ?? string.Empty}," +
                    //     $"\nWorksheet {order.Materials[i].Worksheet ?? string.Empty}," +
                    //     $"\nLine {order.Materials[i].Line}," +
                    //     $"\nReferenceBase {order.Materials[i].ReferenceBase ?? string.Empty}, " +
                    //     $"\nReference {order.Materials[i].Reference ?? string.Empty}," +
                    //     $"\nColor {order.Materials[i].Color ?? string.Empty}, " +
                    //     $"\nColorDescription {order.Materials[i].ColorDescription ?? string.Empty}, " +
                    //     $"\nDescription {order.Materials[i].Description ?? string.Empty}," +
                    //     $"\nException: {ex.Message ?? string.Empty}"

                    //  });
                    //       continue;


                    //}
                }

                _progressValue.CurrentValue = _progressValue.CurrentValue + 30; //30 pts x 2  per OrderNumber
                _progressValue.ProgressTask2 = $"Inserting material needs in to PrefSuite... ";
                _progressValue.ProgressTask2 = string.Empty;
                _progress?.Report(_progressValue);

                //ErrorEntity? errorMaterialNeedsMaster = await _prefSuiteDataService.InsertPrefSuiteMaterialNeedsMasterAsync(order.OrderNumber, order.Number, order.Version);
                //if (errorMaterialNeedsMaster != null)
                //{
                //    order.Errors.Add(errorMaterialNeedsMaster);
                //}

                //ErrorEntity? errorMaterialNeeds = await _prefSuiteDataService.InsertPrefSuiteMaterialNeedsAsync(order.OrderNumber, order.Number, order.Version);
                //if (errorMaterialNeeds != null)
                //{
                //    order.Errors.Add(errorMaterialNeeds);
                //}

                //return (order, _progressValue);
            }
            catch (Exception ex)
            {
                //_logService.Error("{$Class}.{$Method}. Unhandled error." +
                //    "\nUnhandled error: writing order {$OrderNumber}." +
                //    "\nException {$Exception}",
                //    nameof(WriteService),
                //    nameof(WriteAsync),
                //    order.OrderNumber,
                //    ex.Message);
                //order.Errors.Add(new ErrorEntity
                //{
                //    OrderNumber = order.OrderNumber,
                //    Level = ErrorLevel.Error,
                //    Code = ErrorCode.ERPWrite_Order,
                //    Message = $"{nameof(WriteService)}.{nameof(WriteAsync)}." +
                //    $"\nUnhandled error: writing order {order.OrderNumber}." +
                //    $"\nException {ex.Message}",

                //});


                //return (order, _progressValue);
            }

        }
    }
}

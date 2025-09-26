// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.DTO;
using a2p.Application.Interfaces;
using a2p.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;

namespace a2p.Infrastructure.Services
{
    public class WriteService : IWriteService
    {

        private readonly ILogService _logService;
        private readonly IFileService _fileService;
        private readonly ISQLRepository _sqlRepository;
        private readonly IExcelService _excelService;
        private readonly IPrefSuiteService _prefSuiteService;
        private IProgress<ProgressValue>? _progress;
        private ProgressValue _progressValue;
        public WriteService(ILogService logService, IPrefSuiteService prefSuiteService, IFileService fileService, IExcelService excelService, ISQLRepository sqlRepository)
        {

            _sqlRepository = sqlRepository;
            _fileService = fileService;
            _excelService = excelService;
            _logService = logService;
            _prefSuiteService = prefSuiteService;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

        }

        public async Task<(A2POrderDto, ProgressValue)> WriteAsync(A2POrderDto a2pOrder, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            _progressValue = progressValue != null ? progressValue : _progressValue;
            _progress = progress != null ? progress : _progress;

            try
            {


                _progressValue.CurrentValue = _progressValue.CurrentValue + 30;   //30pts. x 1 per Orde               
                _progressValue.ProgressTask2 = "Deleting any existsing data import pending orders.....";
                _progressValue.ProgressTask3 = string.Empty;
                _progress?.Report(_progressValue);

                A2PErrorDto? deleteError = await _sqlRepository.DeleteSalesDocumentDataAsync(a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion, a2pOrder.DeleteExistsing);
                if (deleteError != null)
                {
                    a2pOrder.ErrorsWrite.Add(deleteError);
                }



                var Result = await _prefSuiteService.InsertItemsAsync(a2pOrder, _progressValue, _progress);
                var a2pOrdersPref = Result.Item1;
                _progressValue = Result.Item2;


                if (a2pOrdersPref != null)
                {
                    a2pOrder = a2pOrdersPref;
                }

                for (int i = 0; i < a2pOrder.Items.Count; i++)
                {
                    try
                    {
                        _progressValue.CurrentValue = _progressValue.CurrentValue + 10;  //10pts. x 1 per Item
                        _progressValue.ProgressTask2 = $"Inserting items {i + 1} of {a2pOrder.Items.Count} into db...";
                        _progressValue.ProgressTask3 = $"Item # {a2pOrder.Items[i].Item}";

                        _progress?.Report(_progressValue);
                        A2PErrorDto? itemError = await _sqlRepository.InsertOrderItemDTOAsync(a2pOrder.Items[i], a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion, a2pOrder.Items[i].SalesDocumentIdPos);
                        if (itemError != null)
                        {
                            a2pOrder.ErrorsWrite.Add(itemError);
                        }


                    }
                    catch (Exception ex)
                    {

                        _logService.Error("{$Class}.{$Method}. Unhandled error in for loop ." +
                            "\nUnhandled error: writing order {$Order}." +
                            "\nException {$Exception}",
                            nameof(WriteService),
                            nameof(WriteAsync),
                            a2pOrder.Order,
                            ex.Message);

                        a2pOrder.ErrorsWrite.Add(new A2PErrorDto
                        {
                            Order = a2pOrder.Order,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.ERPWrite_Order,
                            Message = $"{nameof(WriteService)}.{nameof(WriteAsync)}." +
                            $"\nUnhandled error: writing order {a2pOrder.Order}." +
                            $"\nException {ex.Message}",

                        });

                    }

                }

                for (int i = 0; i < a2pOrder.Materials.Count; i++)
                {

                    try
                    {
                        _progressValue.CurrentValue = _progressValue.CurrentValue + 1;//1pts. x 1 per material
                        _progressValue.ProgressTask2 = $"Inserting materials {i + 1} of {a2pOrder.Materials.Count} into PrefSuite DB...";
                        _progressValue.ProgressTask3 = $"Material # {a2pOrder.Materials[i].ReferenceBase} {a2pOrder.Materials[i].Color}.";
                        _progress?.Report(_progressValue);
                        A2PErrorDto? ErrorMaterialDTO = await _sqlRepository.InsertOrderMaterialDTOAsync(a2pOrder.Materials[i], a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);

                        if (ErrorMaterialDTO != null)
                        {
                            a2pOrder.ErrorsWrite.Add(ErrorMaterialDTO);
                            continue;
                        }



                        A2PErrorDto? errorPrefColor = await _sqlRepository.InsertPrefSuiteColorAsync(a2pOrder.Materials[i]);
                        if (errorPrefColor != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefColor);
                            continue;
                        }
                        int GetColorConfiguration = await _sqlRepository.GetPrefSuiteColorConfigurationAsync(a2pOrder.Materials[i].Color);

                        A2PErrorDto? errorColorConfiguration = await _sqlRepository.InsertPrefSuiteColorConfigurationAsync(a2pOrder.Materials[i]);
                        if (errorColorConfiguration != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorColorConfiguration);
                            continue;
                        }



                        A2PErrorDto? errorPrefMaterialBase = await _sqlRepository.InsertPrefSuiteMaterialBaseAsync(a2pOrder.Materials[i]);
                        if (errorPrefMaterialBase != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefMaterialBase);
                            continue;
                        }
                        A2PErrorDto? errorPrefMaterial = await _sqlRepository.InsertPrefSuiteMaterialAsync(a2pOrder.Materials[i]);
                        if (errorPrefMaterial != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefMaterial);
                            continue;
                        }

                        if (a2pOrder.Materials[i].MaterialType == MaterialType.Profiles)
                        {
                            A2PErrorDto? errorPrefProfile = await _sqlRepository.InsertPrefSuiteMaterialProfileAsync(a2pOrder.Materials[i]);
                            if (errorPrefProfile != null)
                            {
                                a2pOrder.ErrorsWrite.Add(errorPrefProfile);
                                continue;
                            }
                        }

                        if (a2pOrder.Materials[i].MaterialType == MaterialType.Gaskets)
                        {
                            A2PErrorDto? errorPrefMeter = await _sqlRepository.InsertPrefSuiteMaterialMeterAsync(a2pOrder.Materials[i]);
                            if (errorPrefMeter != null)
                            {
                                a2pOrder.ErrorsWrite.Add(errorPrefMeter);
                                continue;
                            }

                        }

                        if (a2pOrder.Materials[i].MaterialType == MaterialType.Piece)
                        {
                            A2PErrorDto? errorPrefPiece = await _sqlRepository.InsertPrefSuiteMaterialPieceAsync(a2pOrder.Materials[i]);
                            if (errorPrefPiece != null)
                            {
                                a2pOrder.ErrorsWrite.Add(errorPrefPiece);
                                continue;
                            }
                        }


                        if (a2pOrder.Materials[i].MaterialType == MaterialType.Glasses || a2pOrder.Materials[i].MaterialType == MaterialType.Panels)
                        {

                            A2PErrorDto? errorPrefSurface = await _sqlRepository.InsertPrefSuiteMaterialSurfaceAsync(a2pOrder.Materials[i]);
                            if (errorPrefSurface != null)
                            {
                                a2pOrder.ErrorsWrite.Add(errorPrefSurface);
                                continue;
                            }

                        }

                        if (a2pOrder.Materials[i].MaterialType != MaterialType.Glasses)
                        {
                            A2PErrorDto? errorUpdateBC = await _sqlRepository.UpdateBCMapping(a2pOrder.Materials[i]);
                            if (errorUpdateBC != null)
                            {
                                a2pOrder.ErrorsWrite.Add(errorUpdateBC);
                                continue;
                            }

                            if (!string.IsNullOrEmpty(a2pOrder.Materials[i].SourceReference))
                            {
                                A2PErrorDto? errorPurchaseData = await _sqlRepository.InsertPrefSuiteMaterialPurchaseDataAsync(a2pOrder.Materials[i]);
                                if (errorPurchaseData != null)
                                {
                                    a2pOrder.ErrorsWrite.Add(errorPurchaseData);
                                    continue;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        _logService.Error(
                        "{$Class}.{$Method}. Unhandled error." +
                        "\nOrder {$Order}," +
                        "\nWorksheet {$Worksheet}," +
                        "\nLine {$Line}," +
                        "\nReferenceBase {$ReferenceBase}, " +
                        "\nReference {$Reference}," +
                        "\nColor {$Color}, " +
                        "\nColor {$ColorDescription}, " +
                        "\nDescription {$Description}," +
                        "\nException: {$Exception}",
                        nameof(WriteService),
                        nameof(WriteAsync),
                        a2pOrder.Materials[i].Order ?? string.Empty,
                        a2pOrder.Materials[i].Worksheet ?? string.Empty,
                        a2pOrder.Materials[i].Line,
                        a2pOrder.Materials[i].ReferenceBase ?? string.Empty,
                        a2pOrder.Materials[i].Reference ?? string.Empty,
                        a2pOrder.Materials[i].Color ?? string.Empty,
                         a2pOrder.Materials[i].ColorDescription ?? string.Empty,
                        a2pOrder.Materials[i].Description ?? string.Empty,
                        ex.Message ?? string.Empty
                       );
                        a2pOrder.ErrorsWrite.Add(new A2PErrorDto()
                        {
                            Order = a2pOrder.Materials[i].Order ?? string.Empty,
                            Level = ErrorLevel.Error,
                            Code = ErrorCode.DatabaseWrite_Material,
                            Message = $"{nameof(WriteService)}.{nameof(WriteAsync)}. Unhandled error." +
                           $"\nOrder {a2pOrder.Materials[i].Order ?? string.Empty}," +
                           $"\nWorksheet {a2pOrder.Materials[i].Worksheet ?? string.Empty}," +
                           $"\nLine {a2pOrder.Materials[i].Line}," +
                           $"\nReferenceBase {a2pOrder.Materials[i].ReferenceBase ?? string.Empty}, " +
                           $"\nReference {a2pOrder.Materials[i].Reference ?? string.Empty}," +
                           $"\nColor {a2pOrder.Materials[i].Color ?? string.Empty}, " +
                           $"\nColorDescription {a2pOrder.Materials[i].ColorDescription ?? string.Empty}, " +
                           $"\nDescription {a2pOrder.Materials[i].Description ?? string.Empty}," +
                           $"\nException: {ex.Message ?? string.Empty}"

                        });
                        continue;

                    }

                }

                _progressValue.CurrentValue = _progressValue.CurrentValue + 30; //30 pts x 2  per Order
                _progressValue.ProgressTask2 = $"Inserting material needs in to PrefSuite... ";
                _progressValue.ProgressTask2 = string.Empty;
                _progress?.Report(_progressValue);

                A2PErrorDto? errorMaterialNeedsMaster = await _sqlRepository.InsertPrefSuiteMaterialNeedsMasterAsync(a2pOrder.Order, a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);
                if (errorMaterialNeedsMaster != null)
                {
                    a2pOrder.ErrorsWrite.Add(errorMaterialNeedsMaster);
                }

                A2PErrorDto? errorMaterialNeeds = await _sqlRepository.InsertPrefSuiteMaterialNeedsAsync(a2pOrder.Order, a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);
                if (errorMaterialNeeds != null)
                {
                    a2pOrder.ErrorsWrite.Add(errorMaterialNeeds);
                }

                return (a2pOrder, _progressValue);
            }
            catch (Exception ex)
            {
                _logService.Error("{$Class}.{$Method}. Unhandled error." +
                    "\nUnhandled error: writing order {$Order}." +
                    "\nException {$Exception}",
                    nameof(WriteService),
                    nameof(WriteAsync),
                    a2pOrder.Order,
                    ex.Message);
                a2pOrder.ErrorsWrite.Add(new A2PErrorDto
                {
                    Order = a2pOrder.Order,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.ERPWrite_Order,
                    Message = $"{nameof(WriteService)}.{nameof(WriteAsync)}." +
                    $"\nUnhandled error: writing order {a2pOrder.Order}." +
                    $"\nException {ex.Message}",

                });


                return (a2pOrder, _progressValue);
            }

        }
    }
}

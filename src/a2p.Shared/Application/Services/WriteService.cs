// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Application.Services
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

        public async Task<A2POrder> WriteAsync(A2POrder a2pOrder, ProgressValue progressValue, IProgress<ProgressValue>? progress = null)
        {

            _progressValue = progressValue != null ? progressValue : _progressValue;
            _progress = progress != null ? progress : _progress;

            try
            {
                A2PError? deleteError = await _sqlRepository.DeleteSalesDocumentDataAsync(a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);
                if (deleteError != null)
                {
                    a2pOrder.ErrorsWrite.Add(deleteError);
                }

                A2POrder a2pOrdersPref = await _prefSuiteService.InsertItemsAsync(a2pOrder);

                if (a2pOrdersPref != null)
                {
                    a2pOrder = a2pOrdersPref;
                }

                for (int i = 0; i < a2pOrder.Items.Count; i++)
                {
                    try
                    {
                        A2PError? itemError = await _sqlRepository.InsertOrderItemDTOAsync(a2pOrder.Items[i], a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion, a2pOrder.Items[i].SalesDocumentIdPos);
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

                        a2pOrder.ErrorsWrite.Add(new A2PError
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

                        A2PError? ErrorMaterialDTO = await _sqlRepository.InsertOrderMaterialDTOAsync(a2pOrder.Materials[i], a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);
                        if (ErrorMaterialDTO != null)
                        {
                            a2pOrder.ErrorsWrite.Add(ErrorMaterialDTO);
                            continue;
                        }
                        A2PError? errorPrefColor = await _sqlRepository.InsertPrefSuiteColorAsync(a2pOrder.Materials[i]);
                        if (errorPrefColor != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefColor);
                            continue;
                        }
                        int GetColorConfiguration = await _sqlRepository.GetPrefSuiteColorConfigurationAsync(a2pOrder.Materials[i].Color);

                        A2PError? errorColorConfiguration = await _sqlRepository.InsertPrefSuiteColorConfigurationAsync(a2pOrder.Materials[i]);
                        if (errorColorConfiguration != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorColorConfiguration);
                            continue;
                        }
                        A2PError? errorPrefMaterialBase = await _sqlRepository.InsertPrefSuiteMaterialBaseAsync(a2pOrder.Materials[i]);
                        if (errorPrefMaterialBase != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefMaterialBase);
                            continue;
                        }
                        A2PError? errorPrefMaterial = await _sqlRepository.InsertPrefSuiteMaterialAsync(a2pOrder.Materials[i]);
                        if (errorPrefMaterial != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefMaterial);
                            continue;
                        }
                        A2PError? errorPrefProfile = await _sqlRepository.InsertPrefSuiteMaterialProfileAsync(a2pOrder.Materials[i]);
                        if (errorPrefProfile != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefProfile);
                            continue;
                        }
                        A2PError? errorPrefMeter = await _sqlRepository.InsertPrefSuiteMaterialMeterAsync(a2pOrder.Materials[i]);
                        if (errorPrefMeter != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefMeter);
                            continue;
                        }
                        A2PError? errorPrefPiece = await _sqlRepository.InsertPrefSuiteMaterialPieceAsync(a2pOrder.Materials[i]);
                        if (errorPrefPiece != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefPiece);
                            continue;
                        }
                        A2PError? errorPrefSurface = await _sqlRepository.InsertPrefSuiteMateriaSurfaceAsync(a2pOrder.Materials[i]);
                        if (errorPrefSurface != null)
                        {
                            a2pOrder.ErrorsWrite.Add(errorPrefSurface);
                            continue;
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
                        a2pOrder.ErrorsWrite.Add(new A2PError()
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

                A2PError? errorMaterialNeedsMaster = await _sqlRepository.InsertPrefSuiteMaterialNeedsMasterAsync(a2pOrder.Order, a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);
                if (errorMaterialNeedsMaster != null)
                {
                    a2pOrder.ErrorsWrite.Add(errorMaterialNeedsMaster);
                }

                A2PError? errorMaterialNeeds = await _sqlRepository.InsertPrefSuiteMaterialNeedsAsync(a2pOrder.Order, a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);
                if (errorMaterialNeeds != null)
                {
                    a2pOrder.ErrorsWrite.Add(errorMaterialNeeds);
                }

                return a2pOrder;
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
                a2pOrder.ErrorsWrite.Add(new A2PError
                {
                    Order = a2pOrder.Order,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.ERPWrite_Order,
                    Message = $"{nameof(WriteService)}.{nameof(WriteAsync)}." +
                    $"\nUnhandled error: writing order {a2pOrder.Order}." +
                    $"\nException {ex.Message}",

                });
                return a2pOrder;
            }

        }
    }
}

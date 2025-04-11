// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.Interfaces;
using a2p.Shared.Infrastructure.Interfaces;

namespace a2p.Shared.Application.Services
{
    public class PrefSuiteService : IPrefSuiteService
    {
        private readonly ILogService _logService;
        private readonly ISQLService _sqlRepository;

        private readonly Interop.PrefDataManager.IPrefDataSource _prefSuiteOLEDBConnection;
        private ProgressValue _progressValue;
        private IProgress<ProgressValue>? _progress;

        public PrefSuiteService(ILogService logService, ISQLService sqlRepository, DataCache dataCache)
        {
            _logService = logService;
            _sqlRepository = sqlRepository;
            _progressValue = new ProgressValue();
            _progress = new Progress<ProgressValue>();

            _prefSuiteOLEDBConnection = new Interop.PrefDataManager.PrefDataSource();
        }

        public async Task<A2POrder> InsertItemsAsync(A2POrder a2pOrder)
        {
            try
            {
                Interop.PrefSales.SalesDoc salesDoc = new()
                {
                    ConnectionString = _prefSuiteOLEDBConnection.ConnectionString
                };

                //==============================================================================
                // Insert Items 
                //==============================================================================
                await Task.Run(() =>
                {
                    salesDoc.Load(a2pOrder.SalesDocumentNumber, a2pOrder.SalesDocumentVersion);
                    for (int i = 0; i < a2pOrder.Items.Count; i++)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(a2pOrder.Items[i].Item))

                            {
                                continue;
                            }

                            string idPos = Guid.NewGuid().ToString();

                            string Command =
                                $"<cmd:Commands name=\"CommandName\" xmlns:cmd=\"http://www.preference.com/XMLSchemas/2006/PrefCAD.Command\">" +
                                    $"<cmd:Command name=\"Model.SetDimensions\">" +
                                       $"<cmd:Parameter name=\"dimensions\" type=\"string\" value=\"W= {a2pOrder.Items[i].Width};H= {a2pOrder.Items[i].Height}\"/>" +
                                    $"</cmd:Command>" +
                                    $"<cmd:Command name=\"Model.SetModelVariables\">" +
                                        $"<cmd:Parameter name=\"variables\" type=\"list\">" +
                                            $"<cmd:Item type=\"set\">" +
                                                $"<cmd:ItemValue name=\"name\" type=\"string\" value=\"Weight\"/>" +
                                                $"<cmd:ItemValue name=\"namespace\" type=\"string\" value=\"\"/>" +
                                                $"<cmd:ItemValue name=\"value\" type=\"real\" value=\"{Math.Round(a2pOrder.Items[i].Weight, 2)}\"/>" +
                                            $"</cmd:Item> " +
                                        $"</cmd:Parameter></cmd:Command>" +
                                    $"<cmd:Command name=\"Model.Regenerate\"/>" +
                                $"</cmd:Commands>";

                            Interop.PrefSales.SalesDocItem sdi = salesDoc.Items.Add(idPos);

                            sdi.SetCode("ALU_SAPA", false);
                            _ = sdi.ExecuteCommandStr(Command, out string? resultStr, true);
                            sdi.SetUnitPrice(Math.Round((double) a2pOrder.Items[i].Price, 2));
                            sdi.SetUnitCost(Math.Round((double) a2pOrder.Items[i].Cost, 2));
                            sdi.PriceClosed = true;
                            sdi.SetQuantity((int) a2pOrder.Items[i].Quantity);
                            sdi.Fields["Position"].Value = a2pOrder.Items[i].SortOrder.ToString();
                            sdi.Fields["SortOrder"].Value = a2pOrder.Items[i].SortOrder.ToString();
                            sdi.Fields["Description"].Value = a2pOrder.Items[i].Description;
                            sdi.Fields["Nomenclature"].Value = a2pOrder.Items[i].Item.ToString();
                            a2pOrder.Items[i].SalesDocumentIdPos = idPos;

                            _logService.Information($"PrefSuite Service: Item {a2pOrder.Items[i].Item} inserted for order {a2pOrder.Order}.");
                        }
                        catch (Exception ex)
                        {
                            _logService.Error(
                            "{$Class}.{$Method}. Unhandled error." +
                            "\nOrder {$Order}," +
                            "\nWorksheet {$Worksheet}," +
                            "\nLine {$Line}," +
                            "\nItem {Item}, " +
                            "\nDescription {Description}," +
                            "\nException: {$Exception}",
                            nameof(PrefSuiteService),
                            nameof(InsertItemsAsync),
                            a2pOrder.Items[i].Order ?? string.Empty,
                            a2pOrder.Items[i].Worksheet ?? string.Empty,
                            a2pOrder.Items[i].Line,
                           a2pOrder.Items[i].Item ?? string.Empty,
                            a2pOrder.Items[i].Description ?? string.Empty,
                            ex.Message ?? string.Empty
                           );
                            a2pOrder.ErrorsWrite.Add(new A2PError()
                            {
                                Order = a2pOrder.Items[i].Order ?? string.Empty,
                                Level = ErrorLevel.Error,
                                Code = ErrorCode.DatabaseWrite_Material,
                                Message = $"{nameof(PrefSuiteService)}.{nameof(InsertItemsAsync)}. Unhandled error." +
                               $"\nOrder {a2pOrder.Items[i].Order ?? string.Empty}," +
                               $"\nWorksheet {a2pOrder.Items[i].Worksheet ?? string.Empty}," +
                               $"\nLine {a2pOrder.Items[i].Line}," +
                               $"\nReferenceBase {a2pOrder.Items[i].Item ?? string.Empty}, " +
                               $"\nReference {a2pOrder.Items[i].Description ?? string.Empty}," +
                               $"\nException: {ex.Message ?? string.Empty}"
                            });
                            continue;
                        }

                    }

                    salesDoc.Save();
                });
                return a2pOrder;

            }
            catch (Exception ex)
            {
                _logService.Error(
                "{$Class}.{$Method}. Unhandled error." +
                "\nOrder {$Order}," +
                "\nException: {$Exception}",
                nameof(PrefSuiteService),
                nameof(InsertItemsAsync),
                a2pOrder.Order ?? string.Empty,

                ex.Message ?? string.Empty
               );
                a2pOrder.ErrorsWrite.Add(new A2PError()
                {
                    Order = a2pOrder.Order ?? string.Empty,
                    Level = ErrorLevel.Error,
                    Code = ErrorCode.DatabaseWrite_Material,
                    Message = $"{nameof(PrefSuiteService)}.{nameof(InsertItemsAsync)}. Unhandled error." +
                   $"\nOrder {a2pOrder.Order ?? string.Empty}," +
                   $"\nException: {ex.Message ?? string.Empty}"
                });
                return a2pOrder;
            }

        }

    }
}


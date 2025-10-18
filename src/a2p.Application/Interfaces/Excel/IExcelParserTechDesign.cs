// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using a2p.Application.DTOs;
using a2p.Application.Models;

namespace a2p.Application.Interfaces.Excel
{
    public interface IExcelParserTechDesign

    {
        Task<List<ItemDto>> MapItemsAsync(WorksheetDto worksheet, ProgressValue? progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDto>> MapMaterialsAsync(WorksheetDto worksheet, ProgressValue? progressValue, IProgress<ProgressValue>? progress = null);

    }
};

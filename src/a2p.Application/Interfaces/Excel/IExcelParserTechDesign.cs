// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using Application.DTOs;
using Application.Models;

namespace Application.Interfaces.Excel
{
    public interface IExcelParserTechDesign

    {
        Task<List<ItemDto>> ParseItemsAsync(Worksheet worksheet, OrderDto orderDto, ProgressValue? progressValue = null, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDto>> ParseMaterialsAsync(Worksheet worksheet, OrderDto orderDto, ProgressValue? progressValue = null, IProgress<ProgressValue>? progress = null);

    }
};

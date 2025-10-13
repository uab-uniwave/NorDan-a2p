// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using a2p.Domain.Entities;
using a2p.Domain.Models;


namespace a2p.Application.Services.MappingService
{
    public interface IMapperTechDesign

    {
        Task<(List<ItemEntity>, List<ErrorEntity>)> MapItemsAsync(Worksheet worksheet, ProgressValue? progressValue, IProgress<ProgressValue>? progress = null);
        Task<(List<MaterialEntity>, List<ErrorEntity>)> MapMaterialsAsync(Worksheet worksheet, ProgressValue? progressValue, IProgress<ProgressValue>? progress = null);


    }
};

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.DTO;
using a2p.Domain.Entities;
using a2p.Shared.Application.DTO;

namespace a2p.Application.Interfaces
{
    public interface IMapperTechDesign

    {
        Task<(List<ItemDTO>, List<A2PErrorDto>)> MapItemsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<(List<MaterialDTO>, List<A2PErrorDto>)> MapMaterialsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);


    }
};

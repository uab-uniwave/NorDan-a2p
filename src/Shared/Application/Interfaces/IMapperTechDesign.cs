// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Shared.Application.Domain.Entities;
using Shared.Application.DTO;

namespace Shared.Application.Interfaces
{
    public interface IMapperTechDesign

    {
        Task<(List<ItemDTO>, List<A2PError>)> MapItemsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<(List<MaterialDTO>, List<A2PError>)> MapMaterialsAsync(A2PWorksheet a2pWorksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);


    }
};

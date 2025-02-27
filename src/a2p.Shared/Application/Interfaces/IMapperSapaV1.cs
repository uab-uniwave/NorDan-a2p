// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.DTO;

namespace a2p.Shared.Application.Interfaces
{
    public interface IMapperSapaV1

    {

        Task<List<ItemDTO>> MapItemsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task<List<MaterialDTO>> MapMaterialsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
}
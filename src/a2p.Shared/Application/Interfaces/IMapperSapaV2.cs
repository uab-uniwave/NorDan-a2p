// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IMapperSapaV2

    {

        Task MapItemsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
        Task MapMaterialsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);
    }
};

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Services.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{

    public interface IFileService
    {
        //Task<List<OrderEntry>> GetSingleOrderFilesAsync(IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task GetOrdersAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

        // Task<A2POrder> GetOrderFilesAsync(A2POrder order);

        void MoveFilesAsync();

        //Task<bool> IsLockedAsync(string filePath);
    }

}

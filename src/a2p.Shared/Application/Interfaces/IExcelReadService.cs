// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IExcelReadService
    {
        //  Task<List<AppWorksheet>> GetWorksheetListAsync(List<OrderEntry> files, IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);
        Task<List<A2PWorksheet>> GetWorksheetsAsync(A2PFile file, ProgressValue progressValue, IProgress<ProgressValue>? progress);

    }
}
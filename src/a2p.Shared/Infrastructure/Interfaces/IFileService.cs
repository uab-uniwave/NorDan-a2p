// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.Models;
using a2p.Shared.Infrastructure.Services;
using a2p.Shared.Infrastructure.Services.Logger;

namespace a2p.Shared.Infrastructure.Interfaces
{

    public interface IFileService
    {
        //Task<List<OrderEntry>> GetSingleOrderFilesAsync(IProgress<ProgressValue>? progress = null, CancellationToken cancellationToken = default);

        string GetRootFolder();

        string GetFailedFolder();

        string GetSuccessFolder();
        string GetLogFolder();


        List<string>? GetFiles();

        List<A2PFile> GetOrderFiles(string order);

        bool IsLocked(string filePath);

        List<string>? MoveOrderFiles(List<string> files, bool success);



    }
}

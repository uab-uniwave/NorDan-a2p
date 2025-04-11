// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;

namespace a2p.Shared.Application.Interfaces
{
    public interface IWriteService
    {
        Task<A2POrder> WriteAsync(A2POrder a2pOrders, ProgressValue progressValue, IProgress<ProgressValue>?  progress = null);

    }
}

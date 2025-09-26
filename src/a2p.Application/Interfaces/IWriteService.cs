// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.DTO;
using a2p.Domain.Entities;

namespace a2p.Application.Interfaces
{
    public interface IWriteService
    {
        Task<(A2POrderDto, ProgressValue)> WriteAsync(A2POrderDto a2pOrders, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}

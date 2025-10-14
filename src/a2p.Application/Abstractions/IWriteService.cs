// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Domain.Entities;
using a2p.Domain.Models;
namespace a2p.Application.Abstractions
{
    public interface IWriteService
    {
        Task<(OrderEntity, ProgressValue)> WriteAsync(OrderEntity orders, ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}

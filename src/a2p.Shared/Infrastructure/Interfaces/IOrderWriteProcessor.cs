// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IOrderWriteProcessor
    {
        Task WriteAsync(ProgressValue progressValue, IProgress<ProgressValue>? progress = null);

    }
}

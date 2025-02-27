// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Entities;
using a2p.Shared.Application.DTO;
using a2p.Shared.Application.Interfaces;

namespace a2p.Shared.Application.Services
{
    public class MapperSapaV1 : IMapperSapaV1
    {

        public Task<List<ItemDTO>> MapItemsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null) => throw new NotImplementedException();

        public Task<List<MaterialDTO>> MapMaterialsAsync(A2PWorksheet worksheet, ProgressValue progressValue, IProgress<ProgressValue>? progress = null) => throw new NotImplementedException();

    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.DTO;

namespace a2p.Shared.Application.Domain.Entities
{
    public class A2POrder
    {

        //===============================================
        public string Order { get; set; } = string.Empty;
        //===================================================================================
        public SourceAppType SourceAppType { get; set; } = SourceAppType.SapaV2;
        //===================================================================================

        public List<A2PFile> Files { get; set; } = [];
        //===================================================================================
        public List<ItemDTO> Items { get; set; } = [];

        public List<MaterialDTO> Materials { get; set; } = [];
        //===================================================================================
        public List<A2PError> ErrorsRead { get; set; } = [];
        public List<A2PError> ErrorsWrite { get; set; } = [];
        //===================================================================================
        public int SalesDocumentNumber { get; set; } = -1;
        public int SalesDocumentVersion { get; set; } = -1;
        public int SalesDocumentState { get; set; } = 0;
        //===================================================================================
        public string? Currency { get; set; }
        public double ExchangeRate { get; set; }
        //===================================================================================
        public bool Import { get; set; } = false;

    }
}

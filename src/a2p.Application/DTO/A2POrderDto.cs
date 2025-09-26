// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Application.DTO;
using a2p.Application.Domain.Entities;
using a2p.Domain.Enums;

namespace a2p.Application.DTO
{
    public class A2POrderDto
    {

        //===============================================
        public string Order { get; set; } = string.Empty;
        //===================================================================================
        public SourceAppType SourceAppType { get; set; } = SourceAppType.TechDesign;
        //===================================================================================
        public bool DeleteExistsing { get; set; } = true;
        //===================================================================================

        public List<A2PFileDto> Files { get; set; } = [];
        //===================================================================
        public List<A2PItemDto> Items { get; set; } = [];

        public List<A2PMaterialDto> Materials { get; set; } = [];
        //===================================================================================
        public List<A2PErrorDto> ErrorsRead { get; set; } = [];
        public List<A2PErrorDto> ErrorsWrite { get; set; } = [];
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

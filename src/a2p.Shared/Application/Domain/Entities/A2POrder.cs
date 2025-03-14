// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

using a2p.Shared.Application.Domain.Enums;
using a2p.Shared.Application.DTO;

namespace a2p.Shared.Application.Domain.Entities
{
    public class A2POrder
    {

        //===============================================
        [Required] required public string Order { get; set; }
        //===================================================================================
        public SourceAppType SourceAppType { get; set; } = SourceAppType.Unknown;
        //===================================================================================

        public List<A2PFile> Files { get; set; } = [];
        //===================================================================================
        public List<ItemDTO> Items { get; set; } = [];
        public List<MaterialDTO> Materials { get; set; } = [];
        //===================================================================================
        public List<A2POrderError> ReadErrors { get; set; } = [];
        public List<A2POrderError> WriteErrors { get; set; } = [];
        //===================================================================================
        public int SalesDocumentNumber { get; set; }
        public int SalesDocumentVersion { get; set; }
        public int SalesDocumentState { get; set; }
        //===================================================================================
        public string? Currency { get; set; }
        public decimal ExchangeRate { get; set; }
        //===================================================================================


    }
}

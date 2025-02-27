// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Application.Models.Models
{
    public class MaterialSapa_v1 : BaseMaterial
    {
        public string CustomField1 { get; set; } = string.Empty;
        public string CustomField2 { get; set; } = string.Empty;
        public string CustomField3 { get; set; } = string.Empty;

        public string QuantityOrdered { get; set; } = "0";
        public string QuantityRequired { get; set; } = "0";
        public string Waste { get; set; } = "0";
        public string Currency { get; set; } = "0";
    }
}

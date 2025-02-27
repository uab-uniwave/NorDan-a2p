// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace a2p.Shared.Application.Models.BaseModels
{
    public abstract class BasePanel : BaseMaterial
    {
        public string AreaOrdered { get; set; } = "0";
        public string reaRequired { get; set; } = "0";
        public string Waste { get; set; } = "0";
    }
}

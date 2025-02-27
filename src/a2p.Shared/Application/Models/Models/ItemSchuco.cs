// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Core.Entities
{
    namespace a2p.Shared.Core.Entities
    {
        public class ItemSchuco : BaseItem
        {
            public string Project { get; set; } = string.Empty;// Project nameS
            public string ProfilesSystem { get; set; } = string.Empty; // Profile system
            public string LaborCost { get; set; } = "0";// Labor cost in local currency
            public string Price { get; set; } = "0";// Price in local currency
        }
    }
}


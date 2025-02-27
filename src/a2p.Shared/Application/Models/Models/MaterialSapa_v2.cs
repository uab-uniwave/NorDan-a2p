// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Application.Models.Models
{

    public class MaterialSapa_v2 : BaseMaterial
    {

        public string ArticleType { get; set; } = string.Empty;
        public string SapaArticle { get; set; } = string.Empty;
        public string ColorDescription { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
    }
}

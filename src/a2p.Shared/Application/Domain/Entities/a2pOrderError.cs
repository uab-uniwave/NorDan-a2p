// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Domain.Enums;

namespace a2p.Shared.Application.Domain.Entities
{
    public class A2POrderError
    {
        required public string Order { get; set; }

        required public ErrorLevel Level { get; set; }
        required public ErrorCode Code { get; set; }

        required public string Message { get; set; }

    }
}

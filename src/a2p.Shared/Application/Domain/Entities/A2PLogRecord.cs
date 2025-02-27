// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace a2p.Shared.Application.Domain.Entities
{

    public class A2PLogRecord
    {
        public string Timestamp { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;

        public string Worksheet { get; set; } = string.Empty;
        public string Line { get; set; } = string.Empty;
        public Dictionary<string, object?> Properties { get; set; } = [];
    }
}
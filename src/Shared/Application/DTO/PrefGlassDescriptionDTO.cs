using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Application.DTO
{
    /// <summary>
    /// Represents glass descriptions from NorDan_Glasses table.
    /// </summary>
    public class GlassDescriptionDto
    {
        /// <summary>
        /// Application description for the glass.
        /// </summary>
        [JsonPropertyName("applicationDescription")]
        public string? ApplicationDescription { get; set; }

        /// <summary>
        /// PrefSuite description for the glass.
        /// </summary>
        [JsonPropertyName("prefSuiteDescription")]
        public string? PrefSuiteDescription { get; set; }
    }
}
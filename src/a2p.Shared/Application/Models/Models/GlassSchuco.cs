// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace a2p.Shared.Application.Models.Models
{
    public class GlassSchuco
    {

        public string File { get; set; } = string.Empty; //Excel file name
        public string Worksheet { get; set; } = string.Empty; //Excel worksheet name
        public string Order { get; set; } = string.Empty;
        public string Item { get; set; } = string.Empty;// Item (Column B)
        public string Quantity { get; set; } = "0";//Originaly in chico called Order (Column D)
        public string Width { get; set; } = "0"; // (Column E)
        public string Height { get; set; } = "0";// (Column F)
        public string Area { get; set; } = "0"; //Column H
        public string Weight { get; set; } = "0"; //(Column J)
        public string Price { get; set; } = "0"; // Periceper Priece (Column K)
        public string TotalPrice { get; set; } = "0"; // Price * Quantity (Column K)
        public string Description { get; set; } = string.Empty; //Colunn P
        public string TotalArea { get; set; } = "0";// Area * Quantitu (Column R)

    }
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using a2p.Shared.Application.Models.BaseModels;

namespace a2p.Shared.Application.Models.Models
{

    public class ItemSapa_v2 : BaseItem
    {
        public string Profiles { get; set; } = "0"; // Profile costs
        public string Fittings { get; set; } = "0"; // Fitting costs
        public string GasketsAccessories { get; set; } = "0";// Gasket and accessory costs
        public string AluminumSheet { get; set; } = "0"; // Aluminium sheet costs

        private string surchargeAluminumProfiles { get; set; } = "0";
        public string SetupCostSurfaceTreatment { get; set; } = "0"; // Setup cost for surface treatment
        public string ClientProfilesAccessories { get; set; } = "0"; // Client profiles and accessories costs
        public string Glass { get; set; } = "0";// SortOrder costs
        public string Panel { get; set; } = "0"; // Panel costs
        public string Wages { get; set; } = "0"; // Wages
        public string Hours { get; set; } = "0";// Hours worked
        public string SpecialCost { get; set; } = "0";// Special cos
        public string SellingPrice { get; set; } = "0";// Selling price
        public string GeneralCosts { get; set; } = "0";// General costs
        public string OfferPrice { get; set; } = "0";// Offer price
        public string PriceAdjustage { get; set; } = "0";// Price adjustments
    }
}


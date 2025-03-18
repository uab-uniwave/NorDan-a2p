// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Globalization;

namespace a2p.Shared
{
    public static class GlobalSettings
    {
        static GlobalSettings()
        {
            // Set the culture to en-US (or any other culture you prefer)
            CultureInfo culture = new("en-US");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}

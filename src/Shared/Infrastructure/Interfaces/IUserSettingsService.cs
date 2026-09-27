// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Shared.Application.Models;

namespace Shared.Infrastructure.Interfaces
{
    public interface IUserSettingsService
    {
        SettingsContainer LoadAllSettings();
        AppSettings LoadSettings();
        void SaveSettings(AppSettings updatedAppSettings);
        void SaveConnectionString(string updatedConnectionString);
        string GetSettingsFilePath();
        void SaveSerilogMinimumLevel(string level);
        string LoadSerilogMinimumLevel();
    }
}

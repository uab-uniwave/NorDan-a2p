// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace a2p.Shared.Application.Domain.Enums
{
    public enum ErrorCode
    {
        // ======================================
        // 🟢 Common Errors
        // ======================================
        Application = 1010,

        // ======================================
        // 🔵 Read Process Errors
        // ======================================

        // Filesystem Read Errors (11xx)
        FileSystemRead = 1101,

        // Excel Read Errors (31xx)
        ExcelRead_WorkbookIsEmpty = 3101,
        ExcelRead_WorksheetRow = 3102,
        ExcelRead_WorksheetCell = 3103,

        // Database Read Errors (41xx)
        DatabaseRead_Order = 4101,
        DatabaseRead_Item = 4102,
        DatabaseRead_Material = 4103,
        DatabaseRead_OrderAlreadyImported = 4201,
        DatabaseRead_OrderReferenceNotFound = 4202,

        // ERP Read Errors (51xx)
        ERPRead_Order = 5101,
        ERPRead_Item = 5102,
        ERPRead_Material = 5103,

        // ======================================
        // 🔴 Write Process Errors
        // ======================================

        // Filesystem Write Errors (12xx)
        FileSystemWrite = 1201,

        // Database Write Errors (42xx)
        DatabaseWrite_Order = 4201,
        DatabaseWrite_Item = 4202,
        DatabaseWrite_Material = 4203,

        // Database Delete Errors (43xx)
        DatabaseDelete_Order = 4301,
        DatabaseDelete_Item = 4302,
        DatabaseDelete_Material = 4303,

        // ERP Write Errors (52xx)
        ERPWrite_Order = 5201,
        ERPWrite_Item = 5202,
        ERPWrite_Material = 5203,

        // ERP Delete Errors (53xx)
        ERPDelete_Order = 5301,
        ERPDelete_Item = 5302,
        ERPDelete_Material = 5303,

        // ======================================
        // 🟡 Mapping Errors
        // ======================================
        MappingService_MapOrder = 6101,
        MappingService_MapItem = 6102,
        MappingService_MapMaterial = 6103
    }
}
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

        // Filesystem Read Errors


        // Excel Read Errors 
        ExcelRead_WorkbookIsEmpty = 1201,
        ExcelRead_WorksheetRow = 1202,
        ExcelRead_WorksheetCell = 1203,

        // Database Read Errors 
        DatabaseRead_Order = 1301,
        DatabaseRead_Item = 1302,
        DatabaseRead_Material = 1303,
        DatabaseRead_OrderAlreadyImported = 1304,
        DatabaseRead_OrderReferenceNotFound = 1305,



        // ======================================
        // 🔴 Write Process Errors
        // ======================================



        // Database Write Errors 
        DatabaseWrite_Order = 3201,
        DatabaseWrite_Item = 3202,
        DatabaseWrite_Material = 3203,

        // Database Delete Errors
        DatabaseDelete_Data = 3301,

        // ERP Write Errors 
        ERPWrite_Order = 2401,
        ERPWrite_Item = 2402,
        ERPWrite_Material = 2403,

        // ERP Delete Errors 
        ERPDelete_Order = 2501,
        ERPDelete_Item = 5502,
        ERPDelete_Material = 5503,

        // ======================================
        // 🟡 Mapping Errors
        // ======================================
        MappingService_MapOrder = 6101,
        MappingService_MapItem = 6102,
        MappingService_MapMaterial = 6103,


        // Filesystem Write Errors 
        FileSystemReadWrite = 7101,
    }
}

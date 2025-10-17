namespace a2p.Domain.Enums
{
    public enum ErrorCode
    {
        // ======================================
        // 🟢 Common App Errors
        // ======================================
        Application = 1010,

        // ======================================
        // 🟡 Excel Workbook Format Errors
        // ======================================
        Excel_Read_Workbook_Source_Application_Format_Unknown = 1110,
        Excel_Read_WorkSheet_Format_Unknown = 1120,
        Excel_Read_Workbook_Is_Empty = 1130,
        Excel_Read_Worksheet_Is_Empty = 1140,

        // ======================================
        // 🟡 Excel Read Errors
        // ======================================
        Excel_Read_Worksheet_Row = 1210,
        Excel_Read_Worksheet_Column = 1220,
        Excel_Read_Worksheet_Cell = 1230,

        // ======================================
        // 🟡 Excel Write Errors
        // ======================================
        Excel_Write_Worksheet_Row = 1310,
        Excel_Write_Worksheet_Column = 1320,
        Excel_Write_Worksheet_Cell = 1330,

        // ======================================
        // 🟡 Excel Parsing Errors
        // ======================================
        Excel_Order_Parsing = 1410,//:TODO
        Excel_Items_Parsing = 1420,
        Excel_Material_Parsing = 1430,

        // ======================================
        // 🟡 Mapping Errors
        // ======================================
        Mapping_Order = 1510,
        Mapping_Item = 1520,
        ErrorCode_Mapping_Material = 1530,

        // ======================================
        // 🟡 SQL Connection Errors
        // ======================================
        SQL_Server_Not_Accessible = 1610,
        SQL_Database_Not_Accessible = 1620,
        SQL_User_Permissions = 1630,
        SQL_Data_Write = 1640,
        SQL_Data_Read = 1650,
        SQL_Data_Delete = 1660,

        // ======================================
        // 🟡 Business Proces Errors
        // ======================================
        Business_Process_Order_Not_Found_In_PreSuite = 2010,
        Business_Process_Order_Number_Not_Found = 2020,
        Business_Process_Order_Already_Contains_Items = 2030,
        Business_Process_Order_MaterialNeeds_Calculated = 2040,
        Business_Process_Order_Purcahes_Has_Been_Done = 2050,

        // ======================================
        // 🟡 ERP DB Records Read Errors
        // ======================================
        ERP_Read_Order = 3010,
        ERP_Read_Items = 3020,
        ERP_Read_Materials = 3030,

        // ======================================
        // 🟡 ERP DB Records Write Errors
        // ======================================
        ERP_Write_Order = 3110,
        ERP_Write_Items = 3120,
        ERP_Write_Materilas = 3130,

        // ======================================
        // 🟡 ERP DB Records Delete Errors
        // ======================================
        ERP_Delete_Order = 3210,
        ERP_Delete_Item = 3220,
        ERP_Delete_Materials = 3230,

        // ======================================
        // 🟡 System IO Errors
        // ======================================
        FileSystemReadWrite = 4110,
    }
}

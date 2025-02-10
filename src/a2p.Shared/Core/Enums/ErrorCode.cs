namespace a2p.Shared.Core.Enums
{
    public enum ErrorCode

    {


        Application = 1010,
        //======================================
        File_System_Read = 1101,
        File_System_Write = 1201,
        //======================================
        ReadService_OrderRead = 3101,
        ReadService_OrderNotExistInPrefSuite = 3111,
        ReadService_WorksheetRead = 3201,
        ReadService_WorksheetLineRead = 3301,
        ReadService_WorksheetCellRead = 3401,
        //======================================
        WriteService_ItemWrite = 4101,
        WriteService_MaterialWrite = 4201,
        //======================================
        MappingService_MapMaterial = 5101,
        MappingService_MapItem = 5102,
        //======================================
        SQLRepository_ItemWrite = 6101,
        SQLRepository_MaterialWrite = 6201,

    }
}

# Colors
## Uniwave Brand Colors

| Color         |  Class Name   | RGB           | HEX       | Sample                                                                 |
| ------------- | ------------- | ------------- | --------- | ---------------------------------------------------------------------- |
| Orange Deep   | uwOrangeDeep   | 239, 112, 32  | \#f36f21  | ![#f36f21](https://dummyimage.com/15/f36f21/f36f21.png&text=+)         |
| Orange Bright | uwOrangeBright | 252, 175, 38  | \#fbaf25  | ![#fbaf25](https://dummyimage.com/15/fbaf25/fbaf25.png&text=+)         |
| Orange Light  | uwOrangeLight  | 251, 223, 27  | \#fdd218  | ![#fdd218](https://dummyimage.com/15/fdd218/fdd218.png&text=+)         |
| Grey Dark     | uwGreyDark     | 88, 89, 81    | \#58585b  | ![#58585b](https://dummyimage.com/15/58585b/58585b.png&text=+)         |
| Grey Deep     | uwGreyDeep     | 167, 167, 172 | \#a7a9ab  | ![#a7a9ab](https://dummyimage.com/15/a7a9ab/a7a9ab.png&text=+)         |
| Grey          | uwGrey          | 241, 241, 242 | \#f1f2f2  | ![#f1f2f2](https://dummyimage.com/15/f1f2f2/f1f2f2.png&text=+)         |
| Grey Light    | uwGreyLight    | 248, 248, 249 | \#f8f8fa  | ![#f8f8fa](https://dummyimage.com/15/f8f8fa/f8f8fa.png&text=+)         |




## Nordan-Alu2Prefsuite Colors
| Color         | Class Name    | RGB           | HEX       | Sample                                                                 |
| ------------- | ------------- | ------------- | --------- | ---------------------------------------------------------------------- |
| Grey Dark     | a2pGreyDark   | 56, 57, 60    | \#38393C  | ![#38393C](https://dummyimage.com/15/38393C/38393C.png&text=+)         |
| Grey Deep     | a2pGreyDeep   | 122,123, 124  | \#7A7B7C  | ![#7A7B7C](https://dummyimage.com/15/7A7B7C/7A7B7C.png&text=+)         |


For more information, visit [Our Website](https://dummyimage.com).


##Data transformation objects

Current version used common data transformation objects.

Material transformatio (internally called MaterialDTO). Material data from different materials excel worksheets are unified before storing it into database. 

As input will be used all Sapa V2 material worksheets:

using a2p.Shared.Core.Enums;

namespace a2p.Shared.Core.DTO
{
    public class MaterialDTO : BaseDTO
    {
        public string Worksheet { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
        public int Line { get; set; } = 0;
        public int Column { get; set; } = 0;
        public string Item { get; set; } = string.Empty;  // used just for glass and panels
        public int SortOrder { get; set; } = 0; // used just for glass and panels
        public string Reference { get; set; } = string.Empty;
        public string ColorDescription { get; set; } = string.Empty; //Color description 
        public string Color { get; set; } = string.Empty;//
        public string Description { get; set; } = string.Empty;
        public double Width { get; set; } = 0; // used just for glass and panels 
        public double Height { get; set; } = 0; // used just for glass and panels 
        public double Weight { get; set; } = 0; // used just for glass and panels 
        public double Area { get; set; } = 0; // used just for glass and panels, could be used for other materials painting surface calculatiuon 
        public int Quantity { get; set; } = 0;
        public double QuantityPerPackage { get; set; } = 0; //Not used for panells and glasses in case of Bars. Barr Lenght 
        public double QuantityOrdered { get; set; } = 0; //Total Area in case of glass / 
        public double QuantityRequired { get; set; } = 0; //in caes glasses normally will be the same as QuantityOrdered value
        public double QuantityLeftover { get; set; } = 0; //in caes glasses normally will 0, in case of panels or bar materials will some specific value
        public double WeightOrdered { get; set; } = 0;
        public double WeightRequired { get; set; } = 0;
        public double WeightLeftover { get; set; } = 0;
        public decimal Price { get; set; } = 0; //Default price per unit (In case of glass, per piece) 
        public decimal PriceOrderd { get; set; } = 0;
        public decimal PriceRequired { get; set; } = 0;
        public decimal PriceLeftover { get; set; } = 0;
        public string Pallet { get; set; } = string.Empty;
        public string CustomField1 { get; set; } = string.Empty; //Custom field (used for color just SapaV1 materials and panels)
        public string CustomField2 { get; set; } = string.Empty; //Custom field (used for color just SapaV1  materials and panels)
        public string CustomField3 { get; set; } = string.Empty; //Custom field (used for color just SapaV1  materials and panels)
        public string CustomField4 { get; set; } = string.Empty; //Custom field 
        public string CustomField5 { get; set; } = string.Empty; //Custom field 
        


        public decimal SquareMeterPrice { get; set; } = 0; // used just for glass and for panels not cosnider waste for panels. 
        public string SourceReference { get; set; } = string.Empty;
        public string SourceColor { get; set; } = string.Empty;
        public string SourceDescription { get; set; } = string.Empty;

        public WorksheetType Type { get; set; } = WorksheetType.Unknown;







    }
}



   - Sapa
    -Worksheet ND_Profiles 
    -Worksheet ND_Hardware
    -Worksheet ND_Accessories
    -Worksheet ND_Panels 
    -Worksheet ND_Glasses
    -Worksheet ND_Gaskets
    -Worksheet ND_Other 

   - Sapa Leggacy 
    -Worksheet ND_Profiles 
    -Worksheet ND_Hardware
    -Worksheet ND_Accessories
    -Worksheet ND_Panels 
    -Worksheet ND_Glasses
    -Worksheet ND_Gaskets
    -Worksheet ND_Other

  - Schuco 
    WorkSheet


##Item (positions) transformation object.
UNified transformation class (internally called ItemDTO) transform items data from:
- Sapa Legacy 
- Sapa
- Schuco 

export excel files worksheets and store to database using common format 



As output will be used single object called MaterialDTO 









##Sapa legacy
###Materials procssing

In Previouse version of application, materials files exported from Sapa *legacy* and Schuco application where processed and imported in 3 different ways. Processes where distinct based on material kind. 
    -Material (consider all non square materials) - records processed and imported into PrefSuite database, table  `[dbo].[SAPA_RecordsMaterials]`
    -Glass - records processed and imported into  PrefSuite database, table `[dbo].[SAPA_RecordsGlasses]` 
    -Panels - records processed and imported into PrefSuite database, table `[dbo].[SAPA_RecordsPanels]`
###Items (Position)
Items are process and stored in database table `[dbo].[SAPA_RecordsItems]`. 

### Orders 
Orders are mapped and mapping data stored within PrefSuite database in table  `[dbo].[SAPA_OrdersMapping]`.


##Items (Position)

##Schuco
In Previouse version of application, materials files exported from Schuco application where processed and imported in 2 different ways. Processes where distinct based on material kind. 
    -Material (consider all non square materials) - records processed and imported into PrefSuite database, table  `[dbo].[Schuco_RecordsMaterials]`
    -Glass and Panels - records processed and imported into PrefSuite database, table `[dbo].[Schuco_RecordsGlasses]` 




```PowerShell

using System.Drawing;

namespace Uniwave.Colors
{
    public static class UniwaveColorPalette
    {
        public static Color uwOrangeDeep => Color.FromArgb(239, 112, 32);
        public static Color uwOrangeBright => Color.FromArgb(252, 175, 38);
        public static Color uwOrangeLight => Color.FromArgb(251, 223, 27);
        public static Color uwGreyDark => Color.FromArgb(56, 57, 60);
        public static Color uwGreyDeep => Color.FromArgb(167, 167, 172);
        public static Color uwGrey => Color.FromArgb(241, 241, 242);
        public static Color uwGreyLight => Color.FromArgb(248, 248, 249);
        public static Color a2pGreyDark => Color.FromArgb( 56, 57, 60 );
        public static Color a2pGreyDeep => Color.FromArgb(248, 248, 249);

    }
}
```



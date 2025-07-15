# A2P Readme

## Fields lenghts
> [!CAUTION]
> Information about PrefSuite application fields lenghts.

| Type     | Field          | Length |
| :------- | :------------- | :----: |
| Material | Referencia     |   25   |
| Material | ReferenciaBase |   25   |
| Material | Description    |  255   |
| Color    | Name           |   50   |
| Color    | Description    |  120   |
| Item     | Nomenclature   |   25   |
| Item     | Concept        |   50   |
| Item     | Description    |  255   |
| Order    | Reference      |   50   |
| Order    | Internal Code  |   50   |
| Order    | User1          |   50   |
| Order    | User2          |   50   |


## Data transformation objects

Current version of data import from Excel files using common data transformation objects.

Material transformatio (internally called MaterialDTO). Materiales data read from different weksheet and different apps Excel files. After data reading, a2p dterenminate input file and worksheet types, apply different rules to mapp data to  single data transformation model. As resukt independently of source format, output data format stay same.   

### Material DTO properties

| Name               | Data Type   | Default Value | Descriptio                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    |
| :----------------- | :---------- | :-----------: | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| Worksheet          | string      | empty string  | Name of worksheet data was extracted                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| Order              | string (50) | empty string  | Order number form extracted from file name. In case of **Sapa** ``substring before first " " (space)`  .In case **Sapa Legacy** and **Schuco** `substring before first "_" (underscore)`                                                                                                                                                                                                                                                                                                                                                                      |
| Line               | int         |       0       | Line in worksheet                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| Column             | int         |       0       | Not used                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| Item               | string      | empty string  | Used just for glass and panels define Item (position)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| SortOrder          | int         |       0       | Used jus for glass and panels materials.  Define sort order - **sequence** number in items (position) list.                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| Reference          | string (25) | empty string  | Destination Reference (PrefSuite Article Number) <span style="color: red; font-weight: bold;">**!!!**(Not used for glasses)</span>                                                                                                                                                                                                                                                                                                                                                                                                                            |
| Color              | string (50) | empty string  | Color name (not used for glasses)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| ColorDescription   | string      | empty string  | Color description currently used just in panels                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| Description        | string      | empty string  | Reference Description                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         |
| Width              | double      |       0       | Currently used just for glass and panels                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| Height             | double      |       0       | Currently used just for glass and panels                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| Weight             | double      |       0       | Currently used just for glass and panels                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      |
| Area               | double      |       0       | Currently used just for glass and panels, could be used for other materials painting surface calculatiuon                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| Quantity           | int         |       0       | Package units                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| QuantityPerPackage | double      |       0       | Not used for panels and glasses. In case of Bars it is bar lenght in meters.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| QuantityOrdered    | double      |       0       | Total Quantity. In case of glass **should be be treated**  as taotal quantity ogf glass pcs (*not* area).                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| QuantityRequired   | double      |       0       | Values taken from import files. In case in  Value *not exists*, but possible calculate using other fields,  will be calculated.(used by calculation)                                                                                                                                                                                                                                                                                                                                                                                                          |
| QuantityLeftover   | double      |       0       | Values taken from import files. In case in  Value *not exists*, but possible calculate using other fields,  will be calculated.(used by calculation)                                                                                                                                                                                                                                                                                                                                                                                                          |
| WeightOrdered      | double      |       0       | Values taken from import files. In case in  Value *not exists*, but possible calculate using other fields,  will be calculated.(used by calculation)                                                                                                                                                                                                                                                                                                                                                                                                          |
| WeightRequired     | double      |       0       | Values taken from import files. In case in  Value *not exists*, but possible calculate using other fields,  will be calculated.(used by calculation)                                                                                                                                                                                                                                                                                                                                                                                                          |
| WeightLeftover     | double      |       0       | Values taken from import files. In case in  Value *not exists*, but possible calculate using other fields,  will be calculated.                                                                                                                                                                                                                                                                                                                                                                                                                               |
| Waste              | double      |       0       | In case of profiles waste calculated using **cutting lost weigh** (value provided) and **required profile weight** (value calculated). Calculation of waste done considering that provided lost weight included in required weight. Traeting that required quantity claculated using using optiization algorithms. Optimization algorythms normally consider min, max lenght of cuttimg pieces, machines settings, cuttinig saw with and other aspects impacting required lenght. Rrequired length should be enought cut required pieces coonsidering wastes. |
|                    |
| Price              | double      |       0       | Default price per unit (In case of glass, per piece)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| PriceOrderd        | double      |       0       | Total price                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
| PriceRequired      | double      |       0       | Calculated value ``` PriceRequired = QuantityRequired * PriceOrderd / QuantityOrdered ```                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| PriceLeftOver      | double      |       0       | Calculated value ```PriceLeftOver = PriceOrdered - PriceRequired```                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| Pallet             | string      | empty string  | Used for glass or/and panels pllets                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| Material Type      | string      | empty string  |                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| CustomField1       | string      | empty string  | Custom field (used for color just Sapa legacy materials and panels)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           |
| CustomField2       | string      | empty string  | Custom field (used for color just Sapa legacy  materials and panels)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| CustomField3       | string      | empty string  | Custom field (used for color just Sapa legacy  materials and panels)                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          |
| CustomField4       | string      | empty string  | Custom field                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| CustomField5       | string      | empty string  | Custom field                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  |
| SquareMeterPrice   | double      |       0       | Used just for glass and for panels not cosnider waste for panels.                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             |
| SourceReference    | string      | empty string  | Reference as it was in source excel worksheet                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 |
| SourceColor        | string      | empty string  | Color as it was in source excel worksheet                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     |
| SourceDescription  | string      | empty string  | Description as it was in source excel worksheet                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               |
| WorksheetType      | string      |     Type      | Unknown                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |



### Material Type 
Possible values are:
    
 - Unknown = 0
 - Profiles = 1
 - Gaskets = 2
 - Piece = 3
 - Panels = 4
 - Glasses = 5



 ### Worksheet Type 
Possible values are:
  
  
 - Unknown = 0
 - Items_Sapa = 1  
 - Materials_Sapa = 2
 - Glasses_Sapa = 3
 - Panels_Sapa = 4   
---  
 - Items_TechnoDesign = 5  
 - Materials_TechnoDesign = 6
 - Glasses_TechnoDesign = 7 
 - Panels_TechnoDesign = 8  
 ---  
 - Items_Schuco = 9  
 - Materials_Schuco = 10
 - Glasses_Schuco = 11 
   


<summary>



## Waste % calculation  example

### Values we get from worksheet

Example based on profile data of Sapa 

| SAP  Article Id | Surf. order  code | Surf.  description        | Description  | Quant. |  PU   | Sum order | Sum need | Cutting loss in  kg | Tot. surf. | Sum weight | Price | Total price |
| --------------- | ----------------- | ------------------------- | ------------ | :----: | :---: | --------: | -------: | ------------------: | ---------: | ---------: | ----: | ----------: |
| S42759          | N1010.340D        | NCS S 1010-Y50R Gloss  30 | Glazing bead |   2    | 6.60  |     13.20 |    11.28 |                0.36 |       1.24 |       2.51 | 11.44 |      151.02 |

### Input Values*
- Required Qauntity *(Sum need)* = 11.28 m
- Total Qauntity *(*Sum order*) = 13.2 m *(two bars)*
- Lost weight *(*Cutting loss in kg)* = 0.36 kg
- Total Weight *(Sum weight)* =   2.51 kg

### Values Calcultaion

1. #### Calculate profile weight per meter.  

   $Weight = \frac{Total Weigh}{Total Quantity}$.

   $Weight = \frac{2.51kg}{13.2}=0.19015kg/m$.


2. #### Calculate total weight of rerquired profiles.

   $Required Weight = {Weigh}*{Required Quantity}$.

   $Required Weight = {0.19015kg/m}*{11.28m}=2.1449kg$.


3. #### Calculate lost based on lost weigh and profiles required. 


    $Waste=\frac{[Lost Weight]}{[Required Weight]}$.

    $Waste=\frac{0.36kg}{2.1449kg} =0.1678$.
    
    $Perecnt=0.1678*100=16,78\%$.


---

## Sapa Export Files and Worksheets

* Sapa export file `[OrderNumber] SummList.xslx`  

  * Worksheet `ND_Profiles`
  * Worksheet `ND_Hardware` 
  * Worksheet `ND_Accessories`
  * Worksheet `ND_Panels` 
  * Worksheet `ND_Glasses`
  * Worksheet `ND_Gaskets`
  * Worksheet `ND_Other` 



> [!IMPORTANT]
> [OrderNumber]<span style="background-color: red; font-weight: bold;">&nbsp;</span>SummList.xslx
> <br>[OrderNumber]<span style="background-color: red; font-weight: bold;">&nbsp;</span>Price_Details.xslx
><p>Application recognize TechnoDesign files order numbers by space highighted by red color. In case file format will change. Adaptation to recognition algorithm will be file format will be required.</p> 
><p> Example:
><br>  - <span style="color: green; font-weight: bold;">0000Z00000-10</span><span style="background-color: red; font-weight: bold;">&nbsp;</span>Price_Details.xlx
><br>  - <span style="color: green; font-weight: bold;">0000Z00000-10</span><span style="background-color: red; font-weight: bold;">&nbsp;</span>SummList.xslx</p> 
>Green part of number will be used as order number.




## Sapa legacy Export Files and Worksheets
- Sapa legacy glas and panels export file `[OrderNumber]_ALU_*_FillingList_*.xslx`
  - Worksheet `Default glazing supplier_0` used for glasses
  - Worksheet `Metal sheet optimization` used for panels  



- Sapa Legacy rest material export file `[OrderNumber]_ALU_*_MaterialList.xslx`
  - Worksheet `Sapa Accessories_0` for accessories and other materials
  - Worksheet `Sapa Profiles_1` for profiles
  - Worksheet `Default hardware supplier_2` for hardware


  
> [!IMPORTANT]
> [OrderNumber]<span style="background-color: red; font-weight: bold;">-</span>SummList.xslx
> <br>[OrderNumber]<span style="background-color: red; font-weight: bold;">-</span>Price_Details.xslx
><p>Application recognize Sapa (V1) files by hypnens character (-) that is highighted by red color. In case file format will change. Adaptation to recognition algorithm will be required.</p> 
><p> Example: 
><br>  - <span style="color: green; font-weight: bold;">2410Z04652</span><span style="color: red; font-weight: bold;">-</span>ALU_K1_MaterialList_IX1.xslx
><br>  - <span style="color: green; font-weight: bold;">2410Z04652</span><span style="color: red; font-weight: bold;">-</span>ALU_K1_MaterialList_IX1.xslx</p> 
>Green part of number will be used as order number.


> [!NOTE]
> [OrderNumber]ALU_<span style="color: red; font-weight: bold;">\*</span>\_MaterialList_<span style="color: red; font-weight: bold;">\*</span>.xslx.
> <br> Instaed of highlithed  red  "*"  character can be any set of chracters.
> <br> ***Example*** :
> <br> - 2410Z04652-1_ALU_<span style="color: red; font-weight: bold;">K1</span>\_MaterialList_<span style="color: red; font-weight: bold;">IX1</span>.xslx
> <br> - 2410Z04652-1_ALU_<span style="color: red; font-weight: bold;">K1</span>\_CalcSapaLogic_<span style="color: red; font-weight: bold;">IX1</span>.xslx
> 
> 




## Schuco and Worksheets
* Sapa Legacy export file `*_Profile_Summary.xslx`
   * WorkSheet `1 ' for profiles 
   * WorkSheet `2 ` - not used 

## Item (positions) transformation object.

Unified transformation class (internally called ItemDTO) transform items data from:

* Sapa legacy 

- Sapa
- Schuco 

export excel files worksheets and store to database using common format 



As output will be used single object called MaterialDTO



## Glass Reference and Description




# Laminated Glass Code Explanation and Thickness Reference

Laminated glass is composed of two or more glass panes bonded together with one or more interlayers, usually made of PVB (Polyvinyl Butyral). These interlayers enhance the safety, sound insulation, and UV resistance of the glass.

## Understanding Laminated Glass Codes

Laminated glass types are commonly denoted using a code format like `33.1`, `44.2`, `55.4`, etc.

### First Part of the Code: Glass Pane Thickness
The numbers before the dot represent the thickness of the individual glass panes in millimeters.

- For example:  
  - `33.x` → Two **3 mm** glass panes  
  - `44.x` → Two **4 mm** glass panes  
  - `55.x` → Two **5 mm** glass panes  
  - `66.x` → Two **6 mm** glass panes  

### Second Part of the Code: Number of PVB Interlayers
The number **after the dot** indicates how many **PVB interlayers** are used. Each PVB interlayer is typically **0.38 mm** thick.

- `.1` → 1 interlayer → 0.38 mm  
- `.2` → 2 interlayers → 0.76 mm  
- `.4` → 4 interlayers → 1.52 mm  
- `.8` → 8 interlayers → 3.04 mm  

## 📐 Approximate Total Glass Thickness
The total thickness of laminated glass is calculated as:
Total Thickness = Glass Pane 1 + Glass Pane 2 + (Number of Interlayers × 0.38 mm)

---

## 📋 Laminated Glass Code Reference Table

| Glass Code | Structure Description                         | Approximate Total Thickness (mm) |
|------------|-----------------------------------------------|----------------------------------|
| 33.1       | 3 mm + 3 mm + 1 PVB interlayer (0.38 mm)      | 6.38 mm                          |
| 33.2       | 3 mm + 3 mm + 2 PVB interlayers (0.76 mm)     | 6.76 mm                   |
| 44.2       | 4 mm + 4 mm + 2 PVB interlayers (0.76 mm)     | 8.76 mm                          |
| 55.1       | 5 mm + 5 mm + 1 PVB interlayer (0.38 mm)      | 10.38 mm                         |
| 55.2       | 5 mm + 5 mm + 2 PVB interlayers (0.76 mm)     | 10.76 mm                         |
| 55.4       | 5 mm + 5 mm + 4 PVB interlayers (1.52 mm)     | 11.52 mm                         |
| 66.1       | 6 mm + 6 mm + 1 PVB interlayer (0.38 mm)      | 12.38 mm                         |
| 66.2       | 6 mm + 6 mm + 2 PVB interlayers (0.76 mm)     | 12.76 mm                         |
| 66.4       | 6 mm + 6 mm + 4 PVB interlayers (1.52 mm)     | 13.52 mm                         |
| 66.8       | 6 mm + 6 mm + 8 PVB interlayers (3.04 mm)     | 15.04 mm                         |

---

## ✅ Summary

- The **first part** of the code indicates **glass thickness per pane**.
- The **second part** shows **number of PVB layers**, which affect the safety, sound insulation, and overall thickness.
- This coding system helps in quickly identifying the composition and structural properties of laminated safety glass.

<details> 
<summary><span style="font-weight:bold; font-size:20px;">Uniwave Brand Colors</span></summary>

| Color         | Class Name     | RGB           | HEX      | Sample                                                         |
| ------------- | -------------- | ------------- | -------- | -------------------------------------------------------------- |
| Orange Deep   | uwOrangeDeep   | 239, 112, 32  | \#f36f21 | ![#f36f21](https://dummyimage.com/15/f36f21/f36f21.png&text=+) |
| Orange Bright | uwOrangeBright | 252, 175, 38  | \#fbaf25 | ![#fbaf25](https://dummyimage.com/15/fbaf25/fbaf25.png&text=+) |
| Orange Light  | uwOrangeLight  | 251, 223, 27  | \#fdd218 | ![#fdd218](https://dummyimage.com/15/fdd218/fdd218.png&text=+) |
| Grey Dark     | uwGreyDark     | 88, 89, 81    | \#58585b | ![#58585b](https://dummyimage.com/15/58585b/58585b.png&text=+) |
| Grey Deep     | uwGreyDeep     | 167, 167, 172 | \#a7a9ab | ![#a7a9ab](https://dummyimage.com/15/a7a9ab/a7a9ab.png&text=+) |
| Grey          | uwGrey         | 241, 241, 242 | \#f1f2f2 | ![#f1f2f2](https://dummyimage.com/15/f1f2f2/f1f2f2.png&text=+) |
| Grey Light    | uwGreyLight    | 248, 248, 249 | \#f8f8fa | ![#f8f8fa](https://dummyimage.com/15/f8f8fa/f8f8fa.png&text=+) |

</details>


<details> 
<summary><span style="font-weight:bold; font-size:20px;">Nordan-Alu2Prefsuite Colors</span></summary> 

| Color     | Class Name  | RGB          | HEX      | Sample                                                         |
| --------- | ----------- | ------------ | -------- | -------------------------------------------------------------- |
| Grey Dark | a2pGreyDark | 56, 57, 60   | \#38393C | ![#38393C](https://dummyimage.com/15/38393C/38393C.png&text=+) |
| Grey Deep | a2pGreyDeep | 122,123, 124 | \#7A7B7C | ![#7A7B7C](https://dummyimage.com/15/7A7B7C/7A7B7C.png&text=+) |

</details>



## Sapa legacy
### Materials procssing

In Previouse version of application, materials files exported from Sapa *legacy* and Schuco application where processed and imported in 3 different ways. Processes where distinct based on material kind. 
    - Material (consider all non square materials) - records processed and imported into PrefSuite database, table  `[dbo].[SAPA_RecordsMaterials]`
    - Glass - records processed and imported into  PrefSuite database, table `[dbo].[SAPA_RecordsGlasses]` 
    - Panels - records processed and imported into PrefSuite database, table `[dbo].[SAPA_RecordsPanels]`
Items are process and stored in database table `[dbo].[SAPA_RecordsItems]`. 

### Orders 
    - Items (Positions) - records processed and imported into PrefSuite database, table `[dbo].[SAPA_RecordsPositions]`
    - Orders are mapped and mapping data stored within PrefSuite database in table `[dbo].[SAPA_OrdersMapping]`.


## Items (Position)

## Schuco
In Previouse version of application, materials files exported from Schuco application where processed and imported in 2 different ways. Processes where distinct based on material kind. 
    -Material (consider all non square materials) - records processed and imported into PrefSuite database, table  `[dbo].[Schuco_RecordsMaterials]`
    -Glass and Panels - records processed and imported into PrefSuite database, table `[dbo].[Schuco_RecordsGlasses]` 

## Code Examples

### UI

#### Uniwave Brand Colors

<link href="https://cdnjs.cloudflare.com/ajax/libs/prism/1.24.1/themes/prism-tomorrow.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.24.1/prism.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.24.1/components/prism-csharp.min.js"></script>

<button class="github-button" onclick="copyToClipboard()">Copy</button>
<pre id="codeBlock" class="language-csharp">
<code class="language-csharp">
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


        //Project additional colors.
        public static Color a2pGreyDark => Color.FromArgb( 56, 57, 60 );
        public static Color a2pGreyDeep => Color.FromArgb(248, 248, 249);
    }
}
</code>
</pre>

<style>
.github-button {
    display: inline-block;
    padding: 6px 12px;
    margin: 8px 0;
    font-size: 14px;
    font-weight: 600;
    color: #fff;
    background-color: #f36f21;
    border: 1px solid rgba(27,31,35,.15);
    border-radius: 6px;
    cursor: pointer;
    text-align: center;
    vertical-align: middle;
    user-select: none;
}

.github-button:hover {
    background-color:rgb(234, 160, 118);
}
</style>

<script>
function copyToClipboard() {
    var codeBlock = document.getElementById("codeBlock").innerText;
    navigator.clipboard.writeText(codeBlock).then(function() {
        alert('Code copied to clipboard!');
    }, function(err) {
        console.error('Could not copy text: ', err);
    });
}

// Ensure Prism.js is initialized after the DOM is fully loaded
document.addEventListener("DOMContentLoaded", function() {
    Prism.highlightAll();
});
</script>



### Orders


---

#### Order Error example

<link href="https://cdnjs.cloudflare.com/ajax/libs/prism/1.24.1/themes/prism-tomorrow.min.css" rel="stylesheet" />
<script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.24.1/prism.min.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/prism/1.24.1/components/prism-csharp.min.js"></script>

<button class="github-button" onclick="copyToClipboard()">Copy</button>
<pre id="codeBlock" class="language-csharp">
<code class="language-csharp">

     a2pOrder.WriteErrors.Add(new A2PError{Order = a2pOrder.Order,
                                          Level = ErrorLevel.Error,
                                          Code = ErrorCode.MappingService_MapMaterial,
                                          Description = $"Material of Order :{itemDTO.Order}, " +
                                                        $"worksheet {itemDTO.Worksheet}, " +
                                                        $"Line {itemDTO.Line}, " +
                                                        $"Item {itemDTO.Item}" +
                                                        $" - inserted failed. " +
                                                        $"Inserted record count {writeResult}"
                                          });



    </code>
</pre>

<style>
.github-button {
    display: inline-block;
    padding: 6px 12px;
    margin: 8px 0;
    font-size: 14px;
    font-weight: 600;
    color: #fff;
    background-color: #f36f21;
    border: 1px solid rgba(27,31,35,.15);
    border-radius: 6px;
    cursor: pointer;
    text-align: center;
    vertical-align: middle;
    user-select: none;
}

.github-button:hover {
    background-color:rgb(234, 160, 118);
}
</style>

<script>
function copyToClipboard() {
    var codeBlock = document.getElementById("codeBlock").innerText;
    navigator.clipboard.writeText(codeBlock).then(function() {
        alert('Code copied to clipboard!');
    }, function(err) {
        console.error('Could not copy text: ', err);
    });
}

// Ensure Prism.js is initialized after the DOM is fully loaded
document.addEventListener("DOMContentLoaded", function() {
    Prism.highlightAll();
});
</script>

      
## msbuild 
>[!NOTE]
>msbuild a2p.sln -target:clean;rebuild -p:configuration=release;platform=x64;TargetFramework=net8.0-Windows


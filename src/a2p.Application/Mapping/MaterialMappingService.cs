using a2p.Application.DTOs;
using a2p.Domain.Entities;

namespace a2p.Application.Mapping
{

    public class MaterialMappingService : IMaterialMappingService
    {
        public MaterialEntity MapToEntity(ExcelMaterialDto dto)
        {

            try
            {
                MaterialEntity material = new MaterialEntity();
                material.Id = dto.RowId;
                material.OrderId = dto.OrderId;
                material.OrderNumber = dto.OrderNumber;
                material.SalesDocumentNumber = dto.SalesDocumentNumber;
                material.SalesDocumentVersion = dto.SalesDocumentVersion;
                material.Line = dto.Line;
                material.Column = dto.Column;
                material.ItemName = dto.ItemName;
                material.ItemId = dto.ItemId;
                material.SortOrder = dto.SortOrder;
                material.ReferenceBase = dto.ReferenceBase;
                material.Reference = dto.Reference;
                material.Color = dto.Color;
                material.ColorDescription = dto.ColorDescription;
                material.Description = dto.Description;
                material.Width = dto.Width;
                material.Height = dto.Height;
                material.Quantity = dto.Quantity;

                material.PackageQuantity = dto.PackageQuantity;
                material.TotalQuantity = dto.TotalQuantity;
                material.RequiredQuantity = dto.RequiredQuantity;
                material.LeftOverQuantity = dto.LeftOverQuantity;
                material.Weight = dto.Weight;
                material.TotalWeight = dto.TotalWeight;
                material.RequiredWeight = dto.RequiredWeight;
                material.LeftOverWeight = dto.LeftOverWeight;
                material.Area = dto.Area;
                material.TotalArea = dto.TotalArea;
                material.RequiredArea = dto.RequiredArea;
                material.LeftOverArea = dto.LeftOverArea;
                material.Waste = dto.Waste;
                material.Price = dto.Price;
                material.TotalPrice = dto.TotalPrice;
                material.RequiredPrice = dto.RequiredPrice;
                material.LeftOverPrice = dto.LeftOverPrice;
                material.SquareMeterPrice = dto.SquareMeterPrice;
                material.Pallet = dto.Pallet;
                material.CustomField1 = dto.CustomField1;
                material.CustomField2 = dto.CustomField2;
                material.CustomField3 = dto.CustomField3;
                material.CustomField4 = dto.CustomField4;
                material.CustomField5 = dto.CustomField5;
                material.MaterialType = dto.MaterialType;
                material.MaterialType = dto.MaterialType;

                material.SourceReference = dto.SourceReference;
                material.SourceDescription = dto.SourceReference;
                material.SourceColor = dto.SourceColor;
                material.SourceColorDescription = dto.SourceColorDescription;
                material.CommodityCode = dto.CommodityCode;





                return material;


            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error mapping ItemDto to ItemEntity: {ex.Message}", ex);
            }

        }
    }


}

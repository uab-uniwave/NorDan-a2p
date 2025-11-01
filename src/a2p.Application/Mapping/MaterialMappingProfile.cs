// file: src\a2p.Application\Mapping\MaterialMappingProfile.cs
using Application.DTOs;

using AutoMapper;

using Domain.Entities;

namespace Application.Mapping
{
    public class MaterialMappingProfile : Profile
    {
        public MaterialMappingProfile()
        {
            CreateMap<MaterialDto, MaterialEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => src.ItemId))
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
            .ForMember(dest => dest.Worksheet, opt => opt.MapFrom(src => src.Worksheet))
            .ForMember(dest => dest.Line, opt => opt.MapFrom(src => src.Line))
            .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.Column))
            .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder))
            .ForMember(dest => dest.ReferenceBase, opt => opt.MapFrom(src => src.ReferenceBase))
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Reference))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
            .ForMember(dest => dest.ColorDescription, opt => opt.MapFrom(src => src.ColorDescription))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.PackageQuantity, opt => opt.MapFrom(src => src.PackageQuantity))
            .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.TotalQuantity))
            .ForMember(dest => dest.RequiredQuantity, opt => opt.MapFrom(src => src.RequiredQuantity))
            .ForMember(dest => dest.LeftOverQuantity, opt => opt.MapFrom(src => src.LeftOverQuantity))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.TotalWeight, opt => opt.MapFrom(src => src.TotalWeight))
            .ForMember(dest => dest.RequiredWeight, opt => opt.MapFrom(src => src.RequiredWeight))
            .ForMember(dest => dest.LeftOverWeight, opt => opt.MapFrom(src => src.LeftOverWeight))
            .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
            .ForMember(dest => dest.TotalArea, opt => opt.MapFrom(src => src.TotalArea))
            .ForMember(dest => dest.RequiredArea, opt => opt.MapFrom(src => src.RequiredArea))
            .ForMember(dest => dest.LeftOverArea, opt => opt.MapFrom(src => src.LeftOverArea))
            .ForMember(dest => dest.Waste, opt => opt.MapFrom(src => src.Waste))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice))
            .ForMember(dest => dest.RequiredPrice, opt => opt.MapFrom(src => src.RequiredPrice))
            .ForMember(dest => dest.LeftOverPrice, opt => opt.MapFrom(src => src.LeftOverPrice))
            .ForMember(dest => dest.SquareMeterPrice, opt => opt.MapFrom(src => src.SquareMeterPrice))
            .ForMember(dest => dest.Pallet, opt => opt.MapFrom(src => src.Pallet))
            .ForMember(dest => dest.CustomField1, opt => opt.MapFrom(src => src.CustomField1))
            .ForMember(dest => dest.CustomField2, opt => opt.MapFrom(src => src.CustomField2))
            .ForMember(dest => dest.CustomField3, opt => opt.MapFrom(src => src.CustomField3))
            .ForMember(dest => dest.CustomField4, opt => opt.MapFrom(src => src.CustomField4))
            .ForMember(dest => dest.CustomField5, opt => opt.MapFrom(src => src.CustomField5))
            .ForMember(dest => dest.MaterialType, opt => opt.MapFrom(src => src.MaterialType))
            .ForMember(dest => dest.SourceReference, opt => opt.MapFrom(src => src.SourceReference))
            .ForMember(dest => dest.SourceDescription, opt => opt.MapFrom(src => src.SourceDescription))
            .ForMember(dest => dest.SourceColor, opt => opt.MapFrom(src => src.SourceColor))
            .ForMember(dest => dest.SourceColorDescription, opt => opt.MapFrom(src => src.SourceColorDescription))
            .ForMember(dest => dest.CommodityCode, opt => opt.MapFrom(src => src.CommodityCode));

        }
    }
}
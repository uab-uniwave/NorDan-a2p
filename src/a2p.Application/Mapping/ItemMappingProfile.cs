// file: src\a2p.Application\Mapping\ItemMappingProfile.cs
using Application.DTOs;

using AutoMapper;

using Domain.Entities;

namespace Application.Mapping
{
    public class ItemMappingProfile : Profile
    {
        public ItemMappingProfile()
        {
            CreateMap<ItemDto, ItemEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
             .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
            .ForMember(dest => dest.Worksheet, opt => opt.MapFrom(src => src.Worksheet))
            .ForMember(dest => dest.Line, opt => opt.MapFrom(src => src.Line))
            .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.Column))
            .ForMember(dest => dest.ItemName, opt => opt.MapFrom(src => src.ItemName))
            .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Width, opt => opt.MapFrom(src => src.Width))
            .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
            .ForMember(dest => dest.WeightWithoutGlass, opt => opt.MapFrom(src => src.WeightWithoutGlass))
            .ForMember(dest => dest.WeightGlass, opt => opt.MapFrom(src => src.WeightGlass))
            .ForMember(dest => dest.TotalWeight, opt => opt.MapFrom(src => src.TotalWeight))
            .ForMember(dest => dest.TotalWeightWithoutGlass, opt => opt.MapFrom(src => src.TotalWeightWithoutGlass))
            .ForMember(dest => dest.TotalWeightGlass, opt => opt.MapFrom(src => src.TotalWeightGlass))
            .ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area))
            .ForMember(dest => dest.TotalArea, opt => opt.MapFrom(src => src.TotalArea))
            .ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.Hours))
            .ForMember(dest => dest.TotalHours, opt => opt.MapFrom(src => src.TotalHours))
            .ForMember(dest => dest.MaterialCost, opt => opt.MapFrom(src => src.MaterialCost))
            .ForMember(dest => dest.LaborCost, opt => opt.MapFrom(src => src.LaborCost))
            .ForMember(dest => dest.Cost, opt => opt.MapFrom(src => src.Cost))
            .ForMember(dest => dest.TotalMaterialCost, opt => opt.MapFrom(src => src.TotalMaterialCost))
            .ForMember(dest => dest.TotalLaborCost, opt => opt.MapFrom(src => src.TotalLaborCost))
            .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.TotalCost))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
        }
    }
}
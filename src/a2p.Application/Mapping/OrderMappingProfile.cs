using a2p.Application.DTOs;
using a2p.Domain.Entities;

using AutoMapper;

namespace a2p.Application.Mapping
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderDto, OrderEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.ProjectNumber, opt => opt.MapFrom(src => src.ProjectNumber))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
                .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
                .ForMember(dest => dest.ExchangeRateDate, opt => opt.MapFrom(src => src.ExchangeRateDate))

                // Counts
                .ForMember(dest => dest.ItemCount, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Count : 0))
                .ForMember(dest => dest.ErrorCount, opt => opt.MapFrom(src => src.ErrorsDto != null ? src.ErrorsDto.Count : 0))
                // Totals calculated from ItemDto collection (defensive null checks)
                .ForMember(dest => dest.TotalQuantity, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.Quantity) : 0))
                .ForMember(dest => dest.TotalWeight, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.TotalWeight) : 0m))
                .ForMember(dest => dest.TotalWeightWithoutGlass, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.TotalWeightWithoutGlass) : 0m))
                .ForMember(dest => dest.TotalWeightGlass, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.TotalWeightGlass) : 0m))
                .ForMember(dest => dest.TotalArea, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.TotalArea) : 0m))
                .ForMember(dest => dest.TotalHours, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.TotalHours) : 0m))
                .ForMember(dest => dest.TotalMaterialCost, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.TotalMaterialCost) : 0m))
                .ForMember(dest => dest.TotalLaborCost, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.TotalLaborCost) : 0m))
                .ForMember(dest => dest.TotalCost, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.TotalCost) : 0m))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.ItemsDto != null ? src.ItemsDto.Sum(i => i.TotalPrice) : 0m))

                // If you prefer to include material totals (e.g. quantities / prices) from MaterialsDto as well, adjust expressions accordingly.
                // Source app type
                .ForMember(dest => dest.SourceAppType, opt => opt.MapFrom(src => src.SourceAppType));



        }
    }
}
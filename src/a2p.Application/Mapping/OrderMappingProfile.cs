using Application.DTOs;

using AutoMapper;

using Domain.Entities;

namespace Application.Mapping
{
    public class OrderMappingProfile : Profile
    {
        public OrderMappingProfile()
        {
            CreateMap<OrderDto, OrderEntity>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
            .ForMember(dest => dest.ProjectNumber, opt => opt.MapFrom(src => src.ProjectNumber))
            .ForMember(dest => dest.SalesDocumentNumber, opt => opt.MapFrom(src => src.SalesDocument.Number))
            .ForMember(dest => dest.SalesDocumentVersion, opt => opt.MapFrom(src => src.SalesDocument.Version))
            .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
            .ForMember(dest => dest.CustomerTitle, opt => opt.MapFrom(src => src.CustomerTitle))
            .ForMember(dest => dest.CustomerNumber, opt => opt.MapFrom(src => src.CustomerNumber))
            .ForMember(dest => dest.DeliveryAddress, opt => opt.MapFrom(src => src.DeliveryAddress))
            .ForMember(dest => dest.CorrectionAvailableUntil, opt => opt.MapFrom(src => src.CorrectionAvailableUntil))
            .ForMember(dest => dest.ResponsibleManager, opt => opt.MapFrom(src => src.ResponsibleManager))

            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency))
            .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => src.ExchangeRate))
            .ForMember(dest => dest.ExchangeRateDate, opt => opt.MapFrom(src => src.ExchangeRateDate));

        }
    }
}
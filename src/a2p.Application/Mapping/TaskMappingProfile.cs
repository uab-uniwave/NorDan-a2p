// file: src\a2p.Application\Mapping\TaskMappingProfile.cs
using a2p.Application.DTOs;
using a2p.Domain.Entities;

using AutoMapper;

namespace a2p.Application.Mapping
{
    public class TaskMappingProfile : Profile
    {
        public TaskMappingProfile()
        {
            CreateMap<TaskDto, TaskEntity>()

                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.ProjectNumber, opt => opt.MapFrom(src => src.ProjectNumber))
                .ForMember(dest => dest.SalesDocumentNumber, opt => opt.MapFrom(src => src.SalesDocumentNumber))
                .ForMember(dest => dest.SalesDocumentVersion, opt => opt.MapFrom(src => src.SalesDocumentVersion))
                .ForMember(dest => dest.PayloadJson, opt => opt.MapFrom(src => src.PayloadJson))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State))
                .ForMember(dest => dest.ProcessedUTCDateTime, opt => opt.MapFrom(src => src.ProcessedUTCDateTime));

        }
    }
}
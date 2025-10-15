using a2p.Application.DTOs;
using a2p.Domain.Entities;

namespace a2p.Application.Mapping
{
    public interface IMaterialMappingService
    {
        MaterialEntity MapToEntity(ExcelMaterialDto dto);
    }
}

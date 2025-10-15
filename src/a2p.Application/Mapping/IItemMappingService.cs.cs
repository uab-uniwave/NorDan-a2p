using a2p.Application.DTOs;
using a2p.Domain.Entities;

namespace a2p.Application.Mapping
{
    public interface IItemMappingService
    {

        ItemEntity MapToEntity(ExcelItemDto dto);
    }
}

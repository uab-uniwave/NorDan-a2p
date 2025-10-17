using a2p.Application.DTOs;
using a2p.Domain.Entities;
using a2p.Domain.Shared;

namespace a2p.Application.Interfaces.Services
{
    public interface IMaterialService
    {

        // CREATE
        Task<ValidationResult<MaterialEntity>> CreateMaterialAsync(MaterialDto dto);

        // UPDATE
        Task<ValidationResult<MaterialEntity>> UpdateMaterialAsync(MaterialDto dto);
        Task<Result<MaterialEntity>> GetMaterialByIdAsync(Guid id);

        // GET ORDER ITEMS
        Task<PagedResult<IEnumerable<MaterialEntity>?>> GetOrderMaterialsAsync(Guid id, int page, int size);

        // PAGED (repository doesn't expose paged; do simple in-memory paging)
        Task<PagedResult<MaterialEntity>> GetMaterialsAsync(int page, int size);

        // DELETE
        Task<Result<bool>> DeleteMaterialAsync(Guid id);
    }
}

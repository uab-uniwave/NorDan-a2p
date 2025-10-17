using a2p.Domain.Entities;

namespace a2p.Application.Interfaces.Repositories
{
    /// <summary>
    /// Repository interface for managing material entities in the system.
    /// </summary>
    public interface IMaterialRepository
    {
        // CREATE
        Task<MaterialEntity?> CreateMaterialAsync(MaterialEntity material);

        // READ BY ID
        Task<MaterialEntity?> GetMaterialAsync(Guid id);

        // PAGED READ BY ORDER NUMBER
        Task<(IEnumerable<MaterialEntity> Materials, int TotalCount)> GetOrderMaterialsAsync(Guid id, int page, int size);


        // PAGED READ
        Task<(IEnumerable<MaterialEntity> Materials, int TotalCount)> GetMaterialsAsync(int page, int size);

        // UPDATE ALL ORDER DETAILS
        Task<int> UpdateMaterialAsync(MaterialEntity material);


        // DELETE BY ID
        Task<int> DeleteMaterialsdAsync(Guid id);


        // DELETE BY ORDER ID
        Task<int> DeleteMaterialByOrderIdAsync(Guid id);
    }
}

using Domain.Entities;

namespace Application.Interfaces.Repositories
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

        //READ BY ORDER ID
        Task<IEnumerable<MaterialEntity>> GetOrderMaterialsAsync(Guid id);

        // UPDATE 
        Task<int> UpdateMaterialAsync(MaterialEntity material);

        // DELETE BY ID
        Task<int> DeleteMaterialsdAsync(Guid id);

        // DELETE BY ORDER ID
        Task<int> DeleteOrderMaterialsAsync(Guid id);
    }
}

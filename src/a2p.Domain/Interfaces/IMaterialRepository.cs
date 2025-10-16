using a2p.Domain.Entities;

namespace a2p.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for managing material entities in the system.
    /// </summary>
    public interface IMaterialRepository
    {
        /// <summary>
        /// Retrieves a specific material by its unique identifier asynchronously.
        /// </summary>
        /// <param name="rowId">The unique identifier of the material to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the material if found, or null if not found.</returns>
        Task<MaterialEntity?> GetMaterialAsync(Guid id);

        /// <summary>
        /// Retrieves all materials associated with a specific order asynchronously.
        /// </summary>
        /// <param name="rowId">The unique identifier of the order.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of materials if found, or null if not found.</returns>
        Task<IEnumerable<MaterialEntity>?> GetOrderMaterialsAsync(Guid id);

        /// <summary>
        /// Retrieves all materials in the system asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of all materials if found, or null if none exist.</returns>
        Task<IEnumerable<MaterialEntity>?> GetMaterialsAsync();

        /// <summary>
        /// Creates a new material record in the system asynchronously.
        /// </summary>
        /// <param name="material">The material entity to insert.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the newly created material with assigned identifier.</returns>
        Task<MaterialEntity> CreateMaterialAsync(MaterialEntity material);

        /// <summary>
        /// Updates an existing material record in the system asynchronously.
        /// </summary>
        /// <param name="material">The material entity with updated values.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated material if successful, or null if the material was not found.</returns>
        Task<MaterialEntity?> UpdateMaterialAsync(MaterialEntity material);

        /// <summary>
        /// Deletes a material record from the system asynchronously.
        /// </summary>
        /// <param name="rowId">The unique identifier of the material to delete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the identifier of the deleted material.</returns>
        Task<Guid> DeleteMaterialAsync(Guid rowId);
    }
}

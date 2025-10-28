using Application.DTOs;

using Domain.Entities;
using Domain.Shared;

namespace Application.Interfaces.Services
{
    public interface IMaterialService
    {

        // CREATE
        Task<ValidationResult<MaterialEntity>> CreateMaterialAsync(MaterialDto dto);

        // UPDATE
        Task<ValidationResult<MaterialEntity>> UpdateMaterialAsync(MaterialDto dto);

        // GET MATERIAL
        Task<Result<MaterialEntity>> GetMaterialByIdAsync(Guid id);

        // GET ORDER MATERIALS
        Task<Result<IEnumerable<MaterialEntity>?>> GetOrderMaterialsAsync(Guid id);

        // DELETE
        Task<Result<bool>> DeleteMaterialAsync(Guid id);

        // DELETE ORDER MATERIALS
        Task<Result<bool>> DeleteOrderMaterialAsync(Guid id);
    }
}

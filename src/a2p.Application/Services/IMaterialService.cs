using a2p.Application.Models;
using a2p.Domain.Entities;

namespace a2p.Application.Services
{
    public interface IMaterialService
    {
        Task<Result<MaterialEntity>> InsertMaterialAsync(MaterialEntity material);
        Task<Result<MaterialEntity?>> GetMaterialAsync(Guid id);
        Task<Result<IEnumerable<MaterialEntity>?>> GetOrderMaterialsAsync(Guid orderId);
        Task<Result<IEnumerable<MaterialEntity>?>> GetMaterialsAsync();
        Task<Result<MaterialEntity?>> UpdateMaterialAsync(MaterialEntity material);
        Task<Result<Guid>> DeleteMaterialAsync(Guid id);
    }
}

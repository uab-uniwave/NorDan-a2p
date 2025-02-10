using a2p.Shared.Core.DTO;

namespace a2p.Shared.Core.Interfaces.Services
{
    public interface IWriteService
    {
        Task<int> InsertItemAsync(ItemDTO itemDTO, int salesDocumentNumber, int salesDocumentVersion, DateTime dateTime);
        Task<int> InsertMaterialAsync(MaterialDTO materialsDTO, int salesDocumentNumber, int salesDocumentVersion, DateTime dateTime);

        Task<int> DeleteMaterialsAsync(string order);

        Task<int> DeleteItemsAsync(string order);


    }
}

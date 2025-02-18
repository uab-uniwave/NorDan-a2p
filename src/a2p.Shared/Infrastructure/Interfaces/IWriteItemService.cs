using a2p.Shared.Core.DTO;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IWriteItemService
    {
        Task<int> InsertAsync(ItemDTO itemDTO, int salesDocumentNumber, int salesDocumentVersion, DateTime dateTime);
        Task<int> DeleteAsync(string order);


    }
}

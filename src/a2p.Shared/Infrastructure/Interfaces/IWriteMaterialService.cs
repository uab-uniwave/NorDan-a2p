using a2p.Shared.Core.DTO;
using a2p.Shared.Domain.Entities;

namespace a2p.Shared.Infrastructure.Interfaces
{
    public interface IWriteMaterialService
    {


        Task<int> InsertAsync (MaterialDTO materialsDTO, int salesDocumentNumber, int salesDocumentVersion, DateTime dateTime);

        Task<int> DeleteAsync(string order);



    }
}

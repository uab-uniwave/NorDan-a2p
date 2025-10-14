using a2p.Domain.Entities;

namespace a2p.Domain.Interfaces
{
    public interface IOrderRepository
    {

        //============================================================================================================================


        Task<ErrorEntity?> InsertOrderMaterialAsync(MaterialEntity material, int number, int version);
        //============================================================================================================================
        Task<ErrorEntity?> InsertOrderItemAsync(ItemEntity item, int number, int version, string idPos);

    }
}

using System.Data;

namespace a2p.Application.Interfaces.Repositories
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

}

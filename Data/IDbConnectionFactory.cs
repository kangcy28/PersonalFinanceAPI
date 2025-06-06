using System.Data;

namespace PersonalFinanceAPI.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
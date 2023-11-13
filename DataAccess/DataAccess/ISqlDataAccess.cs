using System.Data;

namespace Test.DataAccess.Data
{
    public interface ISqlDataAccess
    {
        IDbConnection OpenConnection();
    }
}
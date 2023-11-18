using System.Data;

namespace Test.DataAccess.DataAccess
{
    public interface ISqlDataAccess
    {
        IDbConnection OpenConnection();
    }
}
using SEP.WebShop.Core.Repositories;
using System.Data.SqlClient;

namespace SEP.WebShop.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(SqlTransaction transaction)
        {
            _transaction = transaction;
        }

        private SqlTransaction _transaction;

        public void Commmit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (SqlException)
            {
                _transaction.Rollback();
            }
        }
    }
}

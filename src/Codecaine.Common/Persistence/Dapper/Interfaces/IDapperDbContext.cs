using System.Data;

namespace Codecaine.Common.Persistence.Dapper.Interfaces
{
    public interface IDapperDbContext
    {
        IDbConnection Connection { get; }
        IDbTransaction? Transaction { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}

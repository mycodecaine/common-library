using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Persistence.Dapper.Interfaces
{
    public interface IDapperUnitOfWork
    {
        void Begin();
        void Commit();
        void Rollback();
    }
}

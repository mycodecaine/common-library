using Codecaine.Common.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Amazon.S3.Util.S3EventNotification;

namespace Codecaine.Common.Persistence
{
    public interface INoSqlRepository<TEntity> : IRepository<TEntity>  
        where TEntity : Entity
    {
    }
}

using Codecaine.Common.Persistence.EfCore;
using Codecaine.Common.Persistence.EfCore.Interfaces;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.Repositories;
using Codecaine.SportService.Infrastructure.DataAccess.Specifications.SportTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Infrastructure.DataAccess.Repositories
{
    internal sealed class SportTypeRepository : Repository<SportType>, ISportTypeRepository
    {
        public SportTypeRepository(IDbContext context) : base(context)
        {
        }

        public Task<bool> IsDuplicateNameAsync(Guid id, string name)
        {
           return AnyAsync(new SportTypeDuplicateNameWithDifferentIdSpecification(name, id));
        }

        public Task<bool> IsNameExistAsync(string name)
        {
            return AnyAsync(new SportTypeWithNameSpecification(name));
        }

       
    }
}

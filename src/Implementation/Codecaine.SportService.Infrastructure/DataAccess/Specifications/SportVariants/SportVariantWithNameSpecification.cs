using Codecaine.Common.Persistence;
using Codecaine.SportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Infrastructure.DataAccess.Specifications.SportVariants
{
    
    internal class SportVariantWithNameSpecification : Specification<SportVariant>
    {
        private readonly string _name;
        private readonly Guid _sportTypeId;

        public SportVariantWithNameSpecification(string name, Guid sportTypeId)
        {
            _name = name;
            _sportTypeId = sportTypeId;
        }
        public override Expression<Func<SportVariant, bool>> ToExpression()
        => sportVariant => sportVariant.Name.Trim().ToLower() == _name.Trim().ToLower() && sportVariant.SportTypeId == _sportTypeId && !sportVariant.Deleted;
    }
}

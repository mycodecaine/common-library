using Codecaine.Common.Persistence;
using Codecaine.SportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Infrastructure.DataAccess.Specifications.SportTypes
{
    internal class SportTypeWithNameSpecification : Specification<SportType>
    {
        private readonly string _name;

        public SportTypeWithNameSpecification(string name)
        {
            _name = name;
        }
        public override Expression<Func<SportType, bool>> ToExpression()
        => sportType => sportType.Name.Trim().ToLower() == _name.Trim().ToLower() && !sportType.Deleted ;
    }
}

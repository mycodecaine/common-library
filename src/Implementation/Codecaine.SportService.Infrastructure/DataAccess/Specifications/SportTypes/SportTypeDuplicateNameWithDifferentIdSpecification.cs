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
    internal class SportTypeDuplicateNameWithDifferentIdSpecification : Specification<SportType>
    {
        private readonly string _name;
        private readonly Guid _id;

        public SportTypeDuplicateNameWithDifferentIdSpecification(string name, Guid id)
        {
            _name = name;
            _id = id;
        }
        public override Expression<Func<SportType, bool>> ToExpression()
        => sportType =>  !sportType.Deleted && sportType.Name.Trim().ToLower() == _name.Trim().ToLower() && sportType.Id != _id;
    }
}

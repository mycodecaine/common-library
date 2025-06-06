using Codecaine.Common.Persistence;
using Codecaine.SportService.Domain.Entities;
using System.Linq.Expressions;

namespace Codecaine.SportService.Infrastructure.DataAccess.Specifications.SportVariants
{


    internal class SportVariantDuplicateNameWithDifferentIdSpecification : Specification<SportVariant>
    {
        private readonly string _name;
        private readonly Guid _id;
        private readonly Guid _sportTypeId;

        public SportVariantDuplicateNameWithDifferentIdSpecification(string name, Guid id, Guid sportTypeId)
        {
            _name = name;
            _id = id;
            _sportTypeId = sportTypeId;
        }
        public override Expression<Func<SportVariant, bool>> ToExpression()
        => sportType => !sportType.Deleted && sportType.Name.Trim().ToLower() == _name.Trim().ToLower() 
            && sportType.Id != _id && sportType.SportTypeId == _sportTypeId ;
    }
}

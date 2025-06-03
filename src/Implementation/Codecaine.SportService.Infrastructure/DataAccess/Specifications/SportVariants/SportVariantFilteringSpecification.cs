using Codecaine.Common.Persistence;
using Codecaine.SportService.Domain.Entities;
using Codecaine.SportService.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Infrastructure.DataAccess.Specifications.SportVariants
{
    internal class SportVariantFilteringSpecification : Specification<SportVariant>
    {
       
        private readonly string? _name;
        private readonly string? _description;
        private readonly Guid? _sportTypeId;
        private readonly string _imageUrl;
        private readonly bool? _isOlympic;



        public SportVariantFilteringSpecification( int pageNumber,int pageSize, string? name, string? description, Guid? sportTypeId, string imageUrl, bool? isOlympic, string sortBy, bool isDesc)
        {

            _name = name;
            _description = description;
            _sportTypeId = sportTypeId;
            _imageUrl = imageUrl;
            _isOlympic = isOlympic;

            ApplyPaging(pageNumber, pageSize);
            AddInclude(sportVariant => sportVariant.SportType);
            AddInclude(sportVariant => sportVariant.PlayerPositions);           
            if (isDesc)
                ApplyOrderByDescending(sortBy);
            else
                ApplyOrderBy(sortBy);
        }

        public override Expression<Func<SportVariant, bool>> ToExpression()
        {
            return sportVariant =>
         (string.IsNullOrEmpty(_name) || sportVariant.Name.Contains(_name)) &&
         (string.IsNullOrEmpty(_description) || sportVariant.Description.Contains(_description)) &&
         (!_sportTypeId.HasValue || sportVariant.SportTypeId == _sportTypeId.Value) &&
         (string.IsNullOrEmpty(_imageUrl) || sportVariant.ImageUrl.Contains(_imageUrl)) &&
         (!_isOlympic.HasValue || sportVariant.IsOlympic == _isOlympic.Value);

        }
    }
}

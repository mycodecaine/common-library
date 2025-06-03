using Codecaine.Common.CQRS.Queries;
using Codecaine.Common.Pagination;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.ViewModels;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Queries.SearchSportVariant
{


    public record SearchSportVariantQuery(int Page, int PageSize, string? Name, string? Description, Guid? SportTypeId, 
        string? ImageUrl, bool? IsOlympic, string? SortBy,bool IsDescending) : IQuery<Maybe<PagedResult<SportVariantViewModel>>>;
}

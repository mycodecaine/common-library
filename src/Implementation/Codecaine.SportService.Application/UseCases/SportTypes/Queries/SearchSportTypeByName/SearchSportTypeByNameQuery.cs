using Codecaine.Common.CQRS.Queries;
using Codecaine.Common.Pagination;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.ViewModels;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Queries.SearchSportTypeByName
{


    public record SearchSportTypeByNameQuery(int Page, int PageSize, string? Name) : IQuery<Maybe<PagedResult<SportTypeViewModel>>>;
}

using Codecaine.Common.CQRS.Queries;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.ViewModels;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Queries.GetSportVariantById
{

    public record GetSportVariantByIdQuery(Guid Id) : IQuery<Maybe<SportVariantViewModel>>;
}

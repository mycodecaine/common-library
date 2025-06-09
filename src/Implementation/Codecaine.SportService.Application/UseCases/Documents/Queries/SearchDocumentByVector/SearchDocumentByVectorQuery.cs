using Codecaine.Common.CQRS.Queries;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.SportService.Application.ViewModels;

namespace Codecaine.SportService.Application.UseCases.Documents.Queries.SearchDocumentByVector
{


    public record SearchDocumentByVectorQuery(string content) : IQuery<Maybe<List<DocumentViewModel>>>;
}

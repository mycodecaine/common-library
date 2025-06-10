using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;

namespace Codecaine.SportService.Application.UseCases.Documents.Commands.CreateDocument
{


    public record CreateDocumentCommand(string Content) : ICommand<Result<CreateDocumentCommandResponse>>;
}

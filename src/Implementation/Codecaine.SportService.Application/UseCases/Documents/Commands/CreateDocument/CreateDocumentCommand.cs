using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;

namespace Codecaine.SportService.Application.UseCases.Documents.Commands.CreateDocument
{


    public record CreateDocumentCommand(string Name, string Description, string Content) : ICommand<Result<CreateDocumentCommandResponse>>;
}

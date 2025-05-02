using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.DeleteSportType
{
    public record DeleteSportTypeCommand(Guid Id) : ICommand<Result>;
}

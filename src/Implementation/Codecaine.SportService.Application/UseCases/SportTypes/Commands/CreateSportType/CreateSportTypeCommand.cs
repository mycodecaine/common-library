using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType
{
    /// <summary>
    /// Command to create a new sport type.
    /// </summary>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="ImageUrl"></param>
    public record CreateSportTypeCommand(string Name, string Description, string ImageUrl) :ICommand<Result<CreateSportTypeCommandResponse>>;
}

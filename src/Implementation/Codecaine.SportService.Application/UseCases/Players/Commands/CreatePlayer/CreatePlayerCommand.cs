using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;

namespace Codecaine.SportService.Application.UseCases.Players.Commands.CreatePlayer
{
  
    public record CreatePlayerCommand(string Name, string Description, string ImageUrl) : ICommand<Result<CreatePlayerCommandResponse>>;
}

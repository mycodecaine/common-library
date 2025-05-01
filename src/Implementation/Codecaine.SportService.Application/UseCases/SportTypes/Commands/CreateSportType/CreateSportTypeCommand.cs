using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType
{   
    public record CreateSportTypeCommand(string Name, string Description, string ImageUrl) :ICommand<Result<CreateSportTypeCommandResponse>>;
}

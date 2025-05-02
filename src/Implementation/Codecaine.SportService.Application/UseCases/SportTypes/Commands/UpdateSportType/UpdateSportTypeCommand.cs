using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.UpdateSportType
{
    /// <summary>
    /// Command to update an existing sport type.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="ImageUrl"></param>
    public record UpdateSportTypeCommand(Guid Id, string Name, string Description, string ImageUrl) : ICommand<Result<UpdateSportTypeCommandResponse>>;
}

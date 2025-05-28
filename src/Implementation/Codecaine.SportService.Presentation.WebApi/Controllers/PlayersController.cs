using Asp.Versioning;
using Codecaine.Common.Errors;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.UseCases.Players.Commands.CreatePlayer;
using Codecaine.SportService.Application.UseCases.Players.Queries.GetPlayerById;
using Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType;
using Codecaine.SportService.Application.UseCases.SportTypes.Queries.GetSportTypeById;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Presentation.WebApi.DTOs.Players;
using Codecaine.SportService.Presentation.WebApi.DTOs.SportTypes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codecaine.SportService.Presentation.WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PlayersController : BaseController
    {
        public PlayersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost()]

        [ProducesResponseType(typeof(CreatePlayerCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] PlayerDto request) =>
         await Result.Create(request, GeneralErrors.UnProcessableRequest)
             .Map(request => new CreatePlayerCommand(request.Name, request.Description, request.ImageUrl))
             .Bind(command => Mediator.Send(command))
             .Match(Ok, BadRequest);


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PlayerViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id) =>
         await Maybe<GetPlayerByIdQuery>
             .From(new GetPlayerByIdQuery(id))
             .Bind(query => Mediator.Send(query))
             .Match(Ok, NotFound);
    }
}

using Asp.Versioning;
using Codecaine.Common.Errors;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType;
using Codecaine.SportService.Application.UseCases.SportTypes.Commands.UpdateSportType;
using Codecaine.SportService.Presentation.WebApi.DTOs.SportTypes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codecaine.SportService.Presentation.WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SportTypesController : BaseController
    {
        public SportTypesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost()]
        [ProducesResponseType(typeof(CreateSportTypeCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] SportTypeDto request) =>
          await Result.Create(request, GeneralErrors.UnProcessableRequest)
              .Map(request => new CreateSportTypeCommand(request.Name,request.Description, request.ImageUrl))
              .Bind(command => Mediator.Send(command))
              .Match(Ok, BadRequest);

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id,[FromBody] SportTypeDto request) =>
         await Result.Create(request, GeneralErrors.UnProcessableRequest)
             .Map(request => new UpdateSportTypeCommand(id,request.Name, request.Description, request.ImageUrl))
             .Bind(command => Mediator.Send(command))
             .Match(Ok, BadRequest);
    }
}

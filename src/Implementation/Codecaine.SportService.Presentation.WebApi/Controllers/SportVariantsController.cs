using Asp.Versioning;
using Codecaine.Common.Errors;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.UseCases.SportVariants.Commands.CreateSportVariant;
using Codecaine.SportService.Presentation.WebApi.DTOs.SportTypes;
using Codecaine.SportService.Presentation.WebApi.DTOs.SportVariants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Codecaine.SportService.Presentation.WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SportVariantsController : BaseController
    {
        public SportVariantsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost()]
        [ProducesResponseType(typeof(CreateSportVariantCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] SportVariantDto request) =>
          await Result.Create(request, GeneralErrors.UnProcessableRequest)
              .Map(request => new CreateSportVariantCommand(request.Name, request.Description, request.ImageUrl,
                  request.IsOlympic,request.SportTypeId, request.RuleScoringSystem, request.RulePlayerCount, 
                  request.RuleGameDuration, request.RuleMaxScore, request.PopularInCountries,
                  request.PlayerPositions))
              .Bind(command => Mediator.Send(command))
              .Match(Ok, BadRequest);
    }
}

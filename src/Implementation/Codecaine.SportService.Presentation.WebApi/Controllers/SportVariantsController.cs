using Asp.Versioning;
using Codecaine.Common.Errors;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.UseCases.SportVariants.Commands.CreateSportVariant;
using Codecaine.SportService.Application.UseCases.SportVariants.Commands.RemovePopularInCountry;
using Codecaine.SportService.Application.UseCases.SportVariants.Commands.UpdateSportVariant;
using Codecaine.SportService.Application.UseCases.SportVariants.Queries.GetSportVariantById;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Presentation.WebApi.DTOs.SportVariants;
using MediatR;
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


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] SportVariantDto request) =>
         await Result.Create(request, GeneralErrors.UnProcessableRequest)
             .Map(request => new UpdateSportVariantCommand(id, request.Name, request.Description, request.ImageUrl,request.IsOlympic,request.SportTypeId, request.RuleScoringSystem,
                 request.RulePlayerCount,request.RuleGameDuration,request.RuleMaxScore,request.PopularInCountries,request.PlayerPositions))
             .Bind(command => Mediator.Send(command))
             .Match(Ok, BadRequest);

        [HttpDelete("{id}/PopularInCountryId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemovePopularInCountry(Guid id, [FromBody] List<Guid> popularInCountryIds) =>
         await Result.Create(popularInCountryIds, GeneralErrors.UnProcessableRequest)
             .Map(request => new RemovePopularInCountryCommand(id, popularInCountryIds))
             .Bind(command => Mediator.Send(command))
             .Match(Ok, BadRequest);


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SportVariantViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id) =>
         await Maybe<GetSportVariantByIdQuery>
             .From(new GetSportVariantByIdQuery(id))
             .Bind(query => Mediator.Send(query))
             .Match(Ok, NotFound);
    }
}

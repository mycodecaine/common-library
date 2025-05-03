using Asp.Versioning;
using Codecaine.Common.Errors;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.UseCases.SportTypes.Commands.CreateSportType;
using Codecaine.SportService.Application.UseCases.SportTypes.Commands.DeleteSportType;
using Codecaine.SportService.Application.UseCases.SportTypes.Commands.UpdateSportType;
using Codecaine.SportService.Application.UseCases.SportTypes.Queries.GetSportTypeById;
using Codecaine.SportService.Application.UseCases.SportTypes.Queries.SearchSportTypeByName;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Presentation.WebApi.DTOs.SportTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codecaine.SportService.Presentation.WebApi.Controllers
{
    /// <summary>  
    /// Controller for managing sport types.  
    /// Provides endpoints to create, update, and delete sport types.  
    /// </summary>  
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SportTypesController : BaseController
    {
        /// <summary>  
        /// Initializes a new instance of the <see cref="SportTypesController"/> class.  
        /// </summary>  
        /// <param name="mediator">The mediator instance for handling requests.</param>  
        public SportTypesController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>  
        /// Creates a new sport type.  
        /// </summary>  
        /// <param name="request">The sport type data transfer object containing the details of the sport type to create.</param>  
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>  
        [HttpPost()]
        [ProducesResponseType(typeof(CreateSportTypeCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] SportTypeDto request) =>
          await Result.Create(request, GeneralErrors.UnProcessableRequest)
              .Map(request => new CreateSportTypeCommand(request.Name, request.Description, request.ImageUrl))
              .Bind(command => Mediator.Send(command))
              .Match(Ok, BadRequest);

        /// <summary>  
        /// Updates an existing sport type.  
        /// </summary>  
        /// <param name="id">The unique identifier of the sport type to update.</param>  
        /// <param name="request">The sport type data transfer object containing the updated details.</param>  
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>  
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] SportTypeDto request) =>
         await Result.Create(request, GeneralErrors.UnProcessableRequest)
             .Map(request => new UpdateSportTypeCommand(id, request.Name, request.Description, request.ImageUrl))
             .Bind(command => Mediator.Send(command))
             .Match(Ok, BadRequest);

        /// <summary>  
        /// Deletes an existing sport type.  
        /// </summary>  
        /// <param name="id">The unique identifier of the sport type to delete.</param>  
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>  
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id) =>
        await Result.Create(new { id }, GeneralErrors.UnProcessableRequest)
            .Map(request => new DeleteSportTypeCommand(id))
            .Bind(command => Mediator.Send(command))
            .Match(Ok, BadRequest);


        /// <summary>
        /// Get by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SportTypeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id) =>
         await Maybe<GetSportTypeByIdQuery>
             .From(new GetSportTypeByIdQuery(id))
             .Bind(query => Mediator.Send(query))
             .Match(Ok, NotFound);

        [HttpGet("search-by-name/page/{page}/pageSize/{pageSize}/name/{name?}")]
        [ProducesResponseType(typeof(SportTypeViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchByName(int page,int pageSize, string name=" ") =>
         await Maybe<SearchSportTypeByNameQuery>
             .From(new SearchSportTypeByNameQuery(page,pageSize,name))
             .Bind(query => Mediator.Send(query))
             .Match(Ok, NotFound);
    }
}

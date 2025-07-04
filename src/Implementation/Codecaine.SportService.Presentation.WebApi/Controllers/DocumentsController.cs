using Asp.Versioning;
using Codecaine.Common.Errors;
using Codecaine.Common.Primitives.Maybe;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.UseCases.Documents.Commands.CreateDocument;
using Codecaine.SportService.Application.UseCases.Documents.Queries.SearchDocumentByVector;
using Codecaine.SportService.Application.UseCases.SportTypes.Queries.GetSportTypeById;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Presentation.WebApi.DTOs.Documents;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Codecaine.SportService.Presentation.WebApi.Controllers
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DocumentsController : BaseController
    {
        public DocumentsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost()]
        [ProducesResponseType(typeof(CreateDocumentCommandResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] DocumentDto request) =>
         await Result.Create(request, GeneralErrors.UnProcessableRequest)
             .Map(request => new CreateDocumentCommand(request.Name,request.Description,request.Content))
             .Bind(command => Mediator.Send(command))
             .Match(Ok, BadRequest);


        [HttpPost("Search")]
        [ProducesResponseType(typeof(List<DocumentViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Search([FromBody] SearchDocumentContentDto request) =>
         await Maybe<SearchDocumentByVectorQuery>
             .From(new SearchDocumentByVectorQuery(request.Content))
             .Bind(query => Mediator.Send(query))
             .Match(Ok, NotFound);
    }
}

using Asp.Versioning;
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
        protected SportVariantsController(IMediator mediator) : base(mediator)
        {
        }
    }
}

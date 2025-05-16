using Codecaine.Common.Abstractions;
using System.Security.Claims;

namespace Codecaine.SportService.Presentation.WebApi.Context
{
    public class RequestContext : IRequestContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
                return Guid.TryParse(userIdClaim?.Value, out var guid) ? guid : Guid.Empty;
            }
        }

        public string UserName
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
            }
        }
    }
}

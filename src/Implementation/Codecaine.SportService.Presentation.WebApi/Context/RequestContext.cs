using Codecaine.Common.Abstractions;

namespace Codecaine.SportService.Presentation.WebApi.Context
{
    public class RequestContext : IRequestContext
    {
        public Guid UserId => Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA");

        public string UserName => "System";
    }
}

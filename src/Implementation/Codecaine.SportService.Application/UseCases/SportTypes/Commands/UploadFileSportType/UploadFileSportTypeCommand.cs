using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.UploadFileSportType
{
    public record UploadFileSportTypeCommand
    (
        Guid Id,
        string FileName,       
        string ContentType,
        Stream FileStream
    ) : ICommand<Result<UploadFileSportTypeCommandResponse>>;
}

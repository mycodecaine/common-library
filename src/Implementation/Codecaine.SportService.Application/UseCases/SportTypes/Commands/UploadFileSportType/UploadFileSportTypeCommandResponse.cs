using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportTypes.Commands.UploadFileSportType
{
    public record UploadFileSportTypeCommandResponse
    (
        string FileName,
        string FileUrl
    );
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.ViewModels
{
    public record PlayerPositionViewModel
    (
        Guid Id ,
        string Name,
        string Description,
        string ImageUrl,
        string Responsibilities
        
    );
}

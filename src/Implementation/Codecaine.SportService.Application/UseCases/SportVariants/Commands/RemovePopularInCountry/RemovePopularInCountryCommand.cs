using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.DTOs;
using Codecaine.SportService.Application.UseCases.SportVariants.Commands.UpdateSportVariant;
using Codecaine.SportService.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Commands.RemovePopularInCountry
{
   
    public record RemovePopularInCountryCommand
    (
        Guid Id,        
        IReadOnlyCollection<Guid> PopularInCountryIds

    )
    : ICommand<Result<RemovePopularInCountryCommandResponse>>;
}

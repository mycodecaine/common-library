using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.UseCases.SportVariants.Commands.CreateSportVariant.DTOs;
using Codecaine.SportService.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Commands.CreateSportVariant
{
    public record CreateSportVariantCommand
    (
        string Name,
        string Description,
        string ImageUrl,
        bool IsOlympic,
        Guid SportTypeId,
        ScoringSystem RuleScoringSystem,
        int RulePlayerCount,
        int? RuleGameDuration,
        int? RuleMaxScore,
        IReadOnlyCollection<PopularInCountryDto> PopularInCountries,
        IReadOnlyCollection<PlayerPositionDto> PlayerPositions


    )
    :ICommand<Result<CreateSportVariantCommandResponse>>;
}

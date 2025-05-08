using Codecaine.Common.CQRS.Commands;
using Codecaine.Common.Primitives.Result;
using Codecaine.SportService.Application.DTOs;
using Codecaine.SportService.Domain.Enumerations;

namespace Codecaine.SportService.Application.UseCases.SportVariants.Commands.UpdateSportVariant
{

    public record UpdateSportVariantCommand
    (
        Guid Id,
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
    : ICommand<Result<UpdateSportVariantCommandResponse>>;
}

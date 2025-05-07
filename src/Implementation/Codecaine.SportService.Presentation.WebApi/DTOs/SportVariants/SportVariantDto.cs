using Codecaine.SportService.Application.DTOs;
using Codecaine.SportService.Domain.Enumerations;

namespace Codecaine.SportService.Presentation.WebApi.DTOs.SportVariants
{
    public record SportVariantDto
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
    );
}

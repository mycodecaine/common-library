using Codecaine.SportService.Application.DTOs;
using Codecaine.SportService.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.ViewModels
{
    /// <summary>
    /// Sport Variant ViewModel
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="Name"></param>
    /// <param name="Description"></param>
    /// <param name="ImageUrl"></param>
    /// <param name="IsOlympic"></param>
    /// <param name="SportTypeId"></param>
    /// <param name="RuleScoringSystem"></param>
    /// <param name="RulePlayerCount"></param>
    /// <param name="RuleGameDuration"></param>
    /// <param name="RuleMaxScore"></param>
    /// <param name="PopularInCountries"></param>
    /// <param name="PlayerPositions"></param>
    public record SportVariantViewModel
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
        List<PopularInCountryViewModel> PopularInCountries,
        List<PlayerPositionViewModel> PlayerPositions

    );
}

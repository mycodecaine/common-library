using AutoMapper;
using Codecaine.SportService.Application.ViewModels;
using Codecaine.SportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.Mappers
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain -> ViewModel

            CreateMap<SportType, SportTypeViewModel>();

            CreateMap<SportVariant, SportVariantViewModel>()
               .ForMember(dest => dest.SportTypeName,
                       opt => opt.MapFrom(src => src.SportType.Name))
               // Value Object need to Map Manually
               .ForCtorParam("RuleScoringSystem", opt => opt.MapFrom(src => src.Rules.ScoringSystem))
               .ForCtorParam("RulePlayerCount", opt => opt.MapFrom(src => src.Rules.PlayerCount))
               .ForCtorParam("RuleGameDuration", opt => opt.MapFrom(src => src.Rules.Duration))
               .ForCtorParam("RuleMaxScore", opt => opt.MapFrom(src => src.Rules.MaxScore));

            CreateMap<PlayerPosition, PlayerPositionViewModel>();           

            CreateMap<PopularInCountry, PopularInCountryViewModel>();

            CreateMap<Player, PlayerViewModel>();
             

        }
    }
}

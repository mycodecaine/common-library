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

            CreateMap<SportType, SportTypeViewModel>(); // Domain -> ViewModel

        }
    }
}

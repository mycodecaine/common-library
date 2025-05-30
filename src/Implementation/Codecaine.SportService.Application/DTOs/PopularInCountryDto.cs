﻿using Codecaine.SportService.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.DTOs
{
    public record PopularInCountryDto(CountryCode CountryCode, int Popularity, Guid? Id = null);

}

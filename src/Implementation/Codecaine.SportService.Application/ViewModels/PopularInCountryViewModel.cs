using Codecaine.SportService.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Application.ViewModels
{
   public record PopularInCountryViewModel
       (
       Guid Id,
       CountryCode CountryCode, 
       int Popularity
       
       );
}

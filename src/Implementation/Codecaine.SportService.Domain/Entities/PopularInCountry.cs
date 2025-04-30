using Codecaine.Common.Domain;
using Codecaine.SportService.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Domain.Entities
{
    public class PopularInCountry: Entity
    {
        public CountryCode CountryCode { get; private set; }
        public int Popularity { get; private set; }
        // Parameterless constructor for EF Core
        private PopularInCountry()
        {
        }
        public PopularInCountry(CountryCode countryCode, int popularity) : base(Guid.NewGuid())
        {
            CountryCode = countryCode;
            Popularity = popularity;
        }
        public void Update(CountryCode countryCode, int popularity)
        {
            CountryCode = countryCode;
            Popularity = popularity;
        }
    }
    
}

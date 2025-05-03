using Codecaine.Common.Domain.Events;
using Codecaine.SportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Domain.Events
{
   
    public sealed class SportTypeCreatedDomainEvent : IDomainEvent
    {
        public SportTypeCreatedDomainEvent(SportType sportType) => SportType = sportType;

        public SportType SportType { get; }
    }
}

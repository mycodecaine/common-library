using Codecaine.Common.Domain.Events;
using Codecaine.SportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Domain.Events
{
    internal class SportVariantUpdatedDomainEvent : IDomainEvent
    {
        public SportVariantUpdatedDomainEvent(SportVariant sportVariant) => SportVariant = sportVariant;

        public SportVariant SportVariant { get; }
    }
}

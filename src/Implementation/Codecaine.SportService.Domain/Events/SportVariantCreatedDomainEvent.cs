using Codecaine.Common.Domain.Events;
using Codecaine.SportService.Domain.Entities;
using MassTransit.Transports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Domain.Events
{
    public sealed class SportVariantCreatedDomainEvent:IDomainEvent
    {
        public SportVariantCreatedDomainEvent(SportVariant sportVariant) => SportVariant = sportVariant;

        public SportVariant SportVariant { get; }
    }
}

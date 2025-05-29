using Codecaine.Common.Domain.Events;
using Codecaine.SportService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Domain.Events
{
  

    public sealed class PlayerCreatedDomainEvent : IDomainEvent
    {
        public PlayerCreatedDomainEvent(Player player) => Player = player;

        public Player Player { get; }
    }
}

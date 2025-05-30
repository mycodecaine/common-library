﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.CQRS.Events
{
    /// <summary>
    /// Represents the integration event publisher interface.
    /// </summary>
    public interface IIntegrationEventPublisher
    {
        /// <summary>
        /// Publishes the specified integration event to the message queue.
        /// </summary>
        /// <param name="integrationEvent">The integration event.</param>
        /// <returns>The completed task.</returns>
        void Publish(IIntegrationEvent integrationEvent);
    }
}

using Codecaine.Common.Messaging.MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.SportService.Infrastructure.Messaging
{
    public class CodecaineMessageConsumer : MessageQueueConsumer
    {
        public CodecaineMessageConsumer(ILogger<CodecaineMessageConsumer> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider)
        {
        }
    }
}

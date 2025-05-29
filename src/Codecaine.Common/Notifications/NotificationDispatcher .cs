using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Notifications
{
    public class NotificationDispatcher : INotificationSender
    {
        private readonly Dictionary<NotificationChannel, INotificationChannelSender> _senders;

        public NotificationDispatcher(IEnumerable<INotificationChannelSender> senders)
        {
            _senders = senders.ToDictionary(s => s.Channel, s => s);
        }

        public Task SendAsync(NotificationMessage message, NotificationChannel channel)
        {
            if (!_senders.TryGetValue(channel, out var sender))
                throw new NotSupportedException($"Channel {channel} is not supported.");

            return sender.SendAsync(message);
        }
    }
}

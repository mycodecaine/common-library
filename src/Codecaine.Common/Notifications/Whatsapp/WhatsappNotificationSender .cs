using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Notifications.Whatsapp
{
    public class WhatsappNotificationSender : INotificationChannelSender
    {
        public NotificationChannel Channel => NotificationChannel.Whatsapp;

        public Task SendAsync(NotificationMessage message)
        {
            // Send via Twilio or Meta API
            return Task.CompletedTask;
        }
    }
}

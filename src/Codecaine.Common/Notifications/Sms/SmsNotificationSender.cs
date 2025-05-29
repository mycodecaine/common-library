using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Notifications.Sms
{
    public class SmsNotificationSender : INotificationChannelSender
    {
        public NotificationChannel Channel => NotificationChannel.Sms;

        public Task SendAsync(NotificationMessage message)
        {
            throw new NotImplementedException();
        }
    }
}

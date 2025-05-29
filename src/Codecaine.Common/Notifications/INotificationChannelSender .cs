using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Notifications
{
    public interface INotificationChannelSender
    {
        NotificationChannel Channel { get; }
        Task SendAsync(NotificationMessage message);
    }
}

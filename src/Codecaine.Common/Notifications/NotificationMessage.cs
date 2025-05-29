using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Notifications
{
    public class NotificationMessage
    {
        public string To { get; set; } = string.Empty; // Required, can be an email address or phone number
        public string Subject { get; set; } = string.Empty; 
        public string Body { get; set; } = string.Empty;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codecaine.Common.Notifications.Whatsapp
{
    public class WhatsappSetting
    {
        public const string DefaultSectionName = "TwilioSetting";
        public string AccountSid { get; set; }
        public string AuthToken { get; set; }
        public string FromPhoneNumber { get; set; }


    }
}

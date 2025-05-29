using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Types;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Codecaine.Common.Notifications.Whatsapp
{
    public class WhatsappNotificationSender : INotificationChannelSender
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _fromWhatsapp;
        public WhatsappNotificationSender(IOptions<WhatsappSetting> whatsappSetting)
        {
            var settings = whatsappSetting.Value;
            _accountSid = settings.AccountSid;
            _authToken = settings.AuthToken;
            _fromWhatsapp = settings.FromPhoneNumber;

        }
        public NotificationChannel Channel => NotificationChannel.Whatsapp;

        public Task SendAsync(NotificationMessage message)
        {
            TwilioClient.Init(_accountSid, _authToken);

            var to = new PhoneNumber($"whatsapp:{message.To}");

            return MessageResource.CreateAsync(
                to: to,
                from: new PhoneNumber(_fromWhatsapp),
                body: message.Body
            );
            
        }
    }
}

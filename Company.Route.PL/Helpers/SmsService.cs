using Company.Route.DAL.Models;
using Company.Route.PL.Settings;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Route.PL.Helpers
{
    public class SmsService : ISmsService
    {
        private TwilioSettings _options;

        public SmsService(IOptions<TwilioSettings> options)
        {
            _options = options.Value;
        }

        public MessageResource Send( SmsMessage sms )
        {
            // Credintials
            TwilioClient.Init(_options.AccountSID, _options.AuthToken);

            // build message 
            var message = MessageResource.Create(
                body:sms.Body,
                from:new Twilio.Types.PhoneNumber(_options.TwilioPhoneNumber),
                to:sms.PhoneNumber            
                );
                return message;    
        }
    
    
    
    }
}

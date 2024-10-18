using Company.Route.DAL.Models;
using System.Reflection.Metadata.Ecma335;
using Twilio.Rest.Api.V2010.Account;

namespace Company.Route.PL.Helpers
{
    public interface ISmsService
    {
        public MessageResource Send(SmsMessage sms);

    }
}

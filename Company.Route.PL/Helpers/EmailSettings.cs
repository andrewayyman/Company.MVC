using Company.Route.DAL.Models;
using Company.Route.PL.Settings;
using MailKit.Net.Smtp;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;

namespace Company.Route.PL.Helpers
{
    public class EmailSettings : IMailService
    {

        //// Email Settings Using SMTP CLIENT 
        //public static void SendEmail (Email email)
        //{
        //	var client = new SmtpClient("smtp.gmail.com", 587);
        //	client.EnableSsl = true;
        //	client.Credentials = new NetworkCredential("andrewayman1000@gmail.com", "tnzzrqgmkeqyccmf");
        //	client.Send("andrewayman1000@gmail.com", email.Reciepints,email.Subject , email.Body );
        //      }




        /// Using MailKit
        /// Inject IOptions<MailSettings>
        private readonly MailSettings _options; // to access config settings 
        public EmailSettings( IOptions<MailSettings> options )
        {
            _options = options.Value;
        }
         
        public void SendEmail( Email email )
        {
            // build message 
            var mail = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_options.Email),
                Subject = email.Subject,

            };
            mail.To.Add(MailboxAddress.Parse(email.Reciepints));
            mail.From.Add(new MailboxAddress(_options.DisplayName , _options.Email));


            var builder = new BodyBuilder();

            builder.TextBody = email.Body;
            mail.Body = builder.ToMessageBody();

            // Connect on Mail Server

            using var smtp = new SmtpClient();

            smtp.Connect(_options.Host, _options.Port , MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.Email , _options.Password);

            smtp.Send(mail);

            smtp.Disconnect(true);


        }
    
    
    
    }
}



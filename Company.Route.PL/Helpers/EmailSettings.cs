using Company.Route.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Net.Mail;

namespace Company.Route.PL.Helpers
{
	// Email Settings Using SMTP CLIENT
	public class EmailSettings
	{
		public static void SendEmail (Email email)
		{
			var client = new SmtpClient("smtp.gmail.com", 587);
			client.EnableSsl = true;
			client.Credentials = new NetworkCredential("andrewayman1000@gmail.com", "tnzzrqgmkeqyccmf");
			client.Send("andrewayman1000@gmail.com", email.Reciepints,email.Subject , email.Body );



        }


    }
}

using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Comision.Repository.ImplementationRepository
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Credentials:
            const string credentialUserName = "info@sotreza.ir";
            const string sentFrom = "info@sotreza.ir";
            const string pwd = "jXn3*j08";

            // Configure the client:
            var client =
                new SmtpClient("mail.sotreza.ir")
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false
                };

            // Creatte the credentials:
            var credentials =
                new NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = false;
            client.Credentials = credentials;

            // Create the message:
            var mail =
                new MailMessage(sentFrom, message.Destination)
                {
                    Subject = message.Subject,
                    Body = message.Body
                };

            // Send:
            return  client.SendMailAsync(mail);
        }
    }
}
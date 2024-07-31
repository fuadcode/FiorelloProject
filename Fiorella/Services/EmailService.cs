using Fiorello.Services.Interfaces;
using System.Net;
using System.Net.Mail;
namespace Fiorello.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(List<string> emails, string body, string title, string subject)
        {

            MailMessage mail = new();
            mail.From = new MailAddress("fuadgi@code.edu.az", "Fiorello");
            foreach (var email in emails)
            {
                mail.To.Add(new MailAddress(email));
            }
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = body;

            SmtpClient smtpClient = new()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential("fuadgi@code.edu.az", "avjs wafm ahcx qgvq")
            };
            smtpClient.Send(mail);
        }
    }
}

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace OV.Services.Email
{
    public static class Mailer
    {

        public static MimeMessage GenerateEmailMessage(string to, string subject, string body)
        {
            var login = "login";
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(login));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = body };
            return email;
        }

        public static void SendEmail(MimeMessage email)
        {
            var password = "password";
            var login = "login";
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate(login, password);
            smtp.Send(email);
            smtp.Disconnect(true);
            smtp.Dispose();
        }
    }
}

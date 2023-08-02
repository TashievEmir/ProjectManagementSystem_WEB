using MimeKit.Text;
using MimeKit;
using static System.Net.Mime.MediaTypeNames;
using System;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace ProjectManagementSystem.Services
{
    public class SendEmail
    {
        public void SendGmail(int code, string mail)
        {
            String SendMailFrom = "tes01.star@gmail.com";
            String SendMailTo = mail;
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(SendMailFrom));

            email.To.Add(MailboxAddress.Parse(SendMailTo));

            email.Subject = "Title";

            email.Body = new TextPart(TextFormat.Text) { Text = $" Your verification code: {code} " };

            using var smtp = new SmtpClient();

            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);

            smtp.Authenticate(SendMailFrom, "hwxrwoardwqjpdkn");

            smtp.Send(email);

            smtp.Disconnect(true);
        }
    }
}

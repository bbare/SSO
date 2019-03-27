using System;
using MimeKit;
using MailKit.Net.Smtp;

namespace ServiceLayer.Services
{
    public class EmailService: IEmailService
    {
        private string SmtpServer = Environment.GetEnvironmentVariable("smtpServer");
        private int SmtpPort = Int32.Parse(Environment.GetEnvironmentVariable("smtpPort"));
        private string SmtpUsername = Environment.GetEnvironmentVariable("smtpUsername");
        private string SmtpPassword = Environment.GetEnvironmentVariable("smtpPassword");


        //Function to send an email without formatting
        public MimeMessage CreateEmailPlainBody(string receiverName, string receiverEmail, string emailSubject, string emailBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", "no-reply@kfc-sso.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;
            message.Body = new TextPart("plain")
            {
                Text = emailBody
            };
            return message;
        }

        public void SendEmail(MimeMessage messageToSend)
        {
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(SmtpServer, SmtpPort); 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(SmtpUsername, SmtpPassword);
                emailClient.Send(messageToSend);
                emailClient.Disconnect(true);
            }
        }
    }
}

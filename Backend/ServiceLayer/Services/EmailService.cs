using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using System.Configuration;
using System.Net.Configuration;
using System.Web.Configuration;

namespace ServiceLayer.Services
{
    public class EmailService: IEmailService
    {
        private string SmtpServer = Environment.GetEnvironmentVariable("smtpServer");
        private int SmtpPort = Int32.Parse(Environment.GetEnvironmentVariable("smtpPort"));
        private string SmtpUsername = Environment.GetEnvironmentVariable("smtpUsername");
        private string SmtpPassword = Environment.GetEnvironmentVariable("smtpPassword");


        //Function to send an email without formatting
        public MimeMessage createEmailPlainBody(string receiverName, string receiverEmail, string emailSubject, string emailBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", "support@greetngroup.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;
            message.Body = new TextPart("plain")
            {
                Text = emailBody
            };
            return message;
        }

        public void sendEmail(MimeMessage messageToSend)
        {
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(SmtpServer, SmtpPort); //Need to setup email server before fully implementing sending email
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(SmtpUsername, SmtpPassword);
                emailClient.Send(messageToSend);
                emailClient.Disconnect(true);
            }
        }
    }
}

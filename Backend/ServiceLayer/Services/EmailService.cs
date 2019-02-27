using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

namespace ServiceLayer.Services
{
    public class EmailService: IEmailService
    {
        //Need to setup email server before populating these variables with data
        private const string SmtpServer = "";
        private const int SmtpPort = ;
        private const string SmtpUsername = "";
        private const string SmtpPassword = "";


        //Function to send an email without formatting
        public void sendEmailPlainBody(string receiverName, string receiverEmail, string emailSubject, string emailBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", "support@kfcsso.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;
            message.Body = new TextPart("plain")
            {
                Text = emailBody
            };
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect(SmtpServer, SmtpPort); //Need to setup email server before fully implementing sending email
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(SmtpUsername, SmtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
        }

        //Function to send an email with html formatting
        public void sendEmailHTMLBody(string receiverName, string receiverEmail, string emailSubject, string emailBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", "support@kfcsso.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;
            message.Body = new TextPart()
            {
                Text = emailBody
            };

            using(var emailClient = new SmtpClient())
            {
                emailClient.Connect(SmtpServer, SmtpPort); //Need to setup email server before fully implementing sending email
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                emailClient.Authenticate(SmtpUsername, SmtpPassword);
                emailClient.Send(message);
                emailClient.Disconnect(true);
            }
        }
    }
}

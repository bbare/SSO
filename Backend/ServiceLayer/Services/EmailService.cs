using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;


namespace ServiceLayer.Services
{
    public class EmailService: IEmailService
    {
        //Need to setup email server before populating these variables with data
        private const string SmtpServer = "email-smtp.us-west-2.amazonaws.com";
        private const int SmtpPort = 587;
        private const string SmtpUsername = "AKIAIKGDZB4JHYJW4NHQ";
        private const string SmtpPassword = "BDREGCLUMHfpqXy36czX4B9zQre6IbvCx4CausIn3pgQ";


        //Function to send an email without formatting
        public MimeMessage createEmailPlainBody(string receiverName, string receiverEmail, string emailSubject, string emailBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", "support@kfcsso.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;
            message.Body = new TextPart("plain")
            {
                Text = emailBody
            };
            return message;
        }

        //Function to send an email with html formatting
        public MimeMessage createEmailHTMLBody(string receiverName, string receiverEmail, string emailSubject, string emailBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", "support@kfcsso.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;
            message.Body = new TextPart()
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

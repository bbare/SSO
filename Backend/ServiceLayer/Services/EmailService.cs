using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MimeKit;

namespace ServiceLayer.Services
{
    class EmailService
    {
        //Function to send an email without formatting
        public static void sendEmailPlainBody(string receiverName, string receiverEmail, string emailSubject, string emailBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", "support@kfcsso.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;
            message.Body = new TextPart("plain")
            {
                Text = emailBody
            };
        }

        //Function to send an email with html formatting
        public static void sendEmailHTMLBody(string receiverName, string receiverEmail, string emailSubject, string emailBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", "support@kfcsso.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;
            message.Body = new TextPart()
            {
                Text = emailBody
            };
        }
    }
}

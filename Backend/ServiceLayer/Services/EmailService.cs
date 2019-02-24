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

        //TODO: Move to ResetService.cs
        //Needs work
        private const string resetPasswordBodyStringPart1 = "Hi, ";
        private const string resetPasswordBodyStringPart2 = "You recently requested to reset your password for your KFC account, click the link below to reset it." +
            "The URL is only valid for the next 5 minutes";
        private const string resetPasswordBodyStringPart3 = "If you did not request to reset your password, please contact us by responding to this email."
            + "Thanks, KFC Team";

        //TODO: Move to ResetService.cs
        //Send this email if the user exists in the system
        public void sendResetMessageExists(string receiverName, string receiverEmail, string emailSubject,string resetPasswordURL)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Support", "support@kfcsso.com"));
            message.To.Add(new MailboxAddress(receiverName, receiverEmail));
            message.Subject = emailSubject;
            //The "plain" parameter denotes that the message format type, can be switched to "html" if needed
            message.Body = new TextPart("plain")
            {
                //TODO: Find a better way to create the message body
                Text = resetPasswordBodyStringPart1 + receiverName + resetPasswordBodyStringPart2 + resetPasswordURL + resetPasswordBodyStringPart3
            };
        }

        //Function to send an email
        public void sendEmail(string receiverName, string receiverEmail, string emailSubject, string emailBody)
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
    }
}

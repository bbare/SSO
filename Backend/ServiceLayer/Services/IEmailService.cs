using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface IEmailService
    {
        MimeMessage CreateEmailPlainBody(string receiverName, string receiverEmail, string emailSubject, string emailBodyPlainText);
        void SendEmail(MimeMessage messageToSend);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public interface IEmailService
    {
        void sendEmailPlainBody(string receiverName, string receiverEmail, string emailSubject, string emailBodyPlainText);
        void sendEmailHTMLBody(string receiverName, string receiverEmail, string emailSubject, string emailBodyHTML);
    }
}

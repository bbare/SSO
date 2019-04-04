using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class UserDeletePaylod
    {
        // Included in signature
        public Guid ssoUserId { get; set; }
        public string email { get; set; }
        public long timestamp { get; set; }

        // Excluded from signature
        public string signature { get; set; }

        // Generate string to be signed
        public string PreSignatureString()
        {
            string acc = "";
            acc += "ssoUserId=" + ssoUserId + ";";
            acc += "email=" + email + ";";
            acc += "timestamp=" + timestamp + ";";
            return acc;
        }
    }

    public class UserDeleteRequest
    {
        public string deleteUrl { get; set; }
        public UserDeletePaylod userDeletePayload { get; set; }
    }

    public class UserDeleteRequestService
    {
        public 
    }
}

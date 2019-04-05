using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using DataAccessLayer.Database;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using System.Security.Cryptography;

namespace ServiceLayer.Services
{
    public class UserDeletePayload
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


    public class UserDeleteService
    {
        public async Task<HttpResponseMessage> DeleteFromApp(Application app, Guid userID, string userEmail)
        {
            HttpClient client = new HttpClient();
            UserDeletePayload deletePayload = new UserDeletePayload
            {
                ssoUserId = userID,
                email = userEmail,
                timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds()
            };

            HMACSHA256 hmacsha1 = new HMACSHA256(Encoding.ASCII.GetBytes(app.SharedSecretKey));

            byte[] deletePayloadBuffer = Encoding.ASCII.GetBytes(deletePayload.PreSignatureString());

            byte[] signatureBytes = hmacsha1.ComputeHash(deletePayloadBuffer);

            deletePayload.signature = Convert.ToBase64String(signatureBytes);

            var jsonPayload = new StringContent(JsonConvert.SerializeObject(deletePayload));

            HttpResponseMessage response = await client.PostAsync(app.UserDeletionUrl, jsonPayload);

            return response;

        }
    }
}
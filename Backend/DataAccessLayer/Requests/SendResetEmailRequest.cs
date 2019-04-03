using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Requests
{
    public class SendResetEmailRequest
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string url { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Requests
{
    public class NewPasswordRequest
    {
        [Required]
        public string NewPassword { get; set; }
    }
}

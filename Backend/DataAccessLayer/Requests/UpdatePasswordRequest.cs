using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Requests
{
    public class UpdatePasswordRequest
    {
        [Required]
        public string sessionToken { get; set; }
        [Required]
        public string oldPassword { get; set; }
        [Required]
        public string newPassword { get; set; }
    }
}

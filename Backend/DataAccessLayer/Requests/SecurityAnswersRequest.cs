using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Requests
{
    public class SecurityAnswersRequest
    {
        [Required]
        public string securityA1 { get; set; }
        [Required]
        public string securityA2 { get; set; }
        [Required]
        public string securityA3 { get; set; }
    }
}

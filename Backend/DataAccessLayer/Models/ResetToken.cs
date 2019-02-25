using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    class ResetToken
    {
        [Required, Key] 
        public string resetToken { get; set; }
        [Required, ForeignKey("User")]
        public string userID { get; set; }
        [Required]
        public DateTime expirationTime { get; set; }
    }
}

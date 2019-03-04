using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class PasswordReset
    {
        [Required, Key] 
        public string resetID { get; set; }

        [Required, ForeignKey("User")]
        public Guid userID { get; set; }

        [Required]
        public DateTime expirationTime { get; set; }

        [Required]
        //Variable to keep track of how many attempts were made with this reset ID
        public int resetCount { get; set; }

        [Required]
        //Variable to keep track of user being locked out of resetting password
        public bool disabled { get; set; }
    }
}

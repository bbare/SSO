using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class PasswordReset
    {
        public PasswordReset()
        {
            PasswordResetID = Guid.NewGuid();
            ExpirationTime = DateTime.Now.AddMinutes(5);
            ResetCount = 0;
            Disabled = false;
        }

        [Required, Key]
        public Guid PasswordResetID { get; set; }

        [Required]
        public string ResetToken { get; set; }

        public User User { get; set; }
        [Required, ForeignKey("User")]
        public Guid UserID { get; set; }

        [Required, Column(TypeName = "datetime2"), DataType(DataType.DateTime)]
        public DateTime ExpirationTime { get; set; }

        [Required]
        //Variable to keep track of how many attempts were made with this reset ID
        public int ResetCount { get; set; }

        [Required]
        //Variable to keep track of user being locked out of resetting password
        public bool Disabled { get; set; }

        [Required]
        public bool AllowPasswordReset { get; set; }
    }
}

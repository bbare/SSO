using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Reset
    {
        [Required, Key] 
        public string resetID { get; set; }
        [Required, ForeignKey("User")]
        public Guid userID { get; set; }
        [Required]
        public DateTime expirationTime { get; set; }
    }
}

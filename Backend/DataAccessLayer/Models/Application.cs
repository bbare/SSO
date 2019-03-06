using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Web;

namespace DataAccessLayer.Models
{
    public class Application
    {
        public Application()
        {
        }

        [Key]
        [Required, DataType(DataType.Url)]
        public string Url { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [DataType(DataType.ImageUrl)]
        public string Logo { get; set;}

        public string Description { get; set; }

        [Required, DataType(DataType.Url)]
        public string UserDeletionUrl { get; set; }
    }

    
}

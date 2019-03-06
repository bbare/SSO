using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class ApiKey
    {
        public ApiKey()
        {
            Key = "";
            IsUsed = false;
        }

        [Key]
        public string Key { get; set; }

        [Required, DataType(DataType.Url)]
        public string ApplicationUrl { get; set; }

        [Required]
        public bool IsUsed { get; set; }
    }
}

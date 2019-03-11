using System;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Models
{
    public class ApiKey
    {
        public ApiKey()
        {
            Id = Guid.NewGuid();
            IsUsed = false;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Key { get; set; }

        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public bool IsUsed { get; set; }
    }
}

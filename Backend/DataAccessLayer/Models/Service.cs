using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class Service
    {
        public Service()
        {
            CreatedAt = DateTime.UtcNow;
            Id = Guid.NewGuid();
            Disabled = false;
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public bool Disabled { get; set; }

        [Required, Column(TypeName = "datetime2"), DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }

        [Required, Column(TypeName = "datetime2"), DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
    }
}

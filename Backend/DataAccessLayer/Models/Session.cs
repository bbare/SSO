using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class Session
    {
        public Session()
        {
            CreatedAt = DateTime.UtcNow;
            Id = Guid.NewGuid();
        }

        [Required]
        public string Token { get; set; }
        [Key]
        public Guid Id { get; set; }
        [Required, Column(TypeName = "datetime2"), DataType(DataType.DateTime)]
        public DateTime ExpiresAt { get; set; }

        [Required, Column(TypeName = "datetime2"), DataType(DataType.DateTime)]
        public DateTime UpdatedAt { get; set; }

        [Required, Column(TypeName = "datetime2"), DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Required, ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Models
{
    public class Session
    {
        public static readonly int MINUTES_UNTIL_EXPIRATION = 30;
        public Session()
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            ExpiresAt = DateTime.UtcNow.AddMinutes(MINUTES_UNTIL_EXPIRATION);
            Id = Guid.NewGuid();
            //isExpired = false;
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
        [Index(IsUnique = false)]
        public Guid UserId { get; set; }
        public User User { get; set; }
        //public Boolean isExpired {get; set;}
    }
}

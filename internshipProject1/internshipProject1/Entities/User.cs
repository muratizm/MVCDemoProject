using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace internshipProject1.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(50)]
        public string? FullName { get; set; }


        [Required]
        [StringLength(30)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        [Required]
        public bool Locked { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [StringLength(255)]
        public string ProfileImageFileName { get; set; } = "USERICON.png";

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "user";
    }
}

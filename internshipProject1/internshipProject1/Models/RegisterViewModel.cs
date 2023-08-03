using System.ComponentModel.DataAnnotations;

namespace internshipProject1.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(10)]
        public string Username { get; set; }
        
        [Required]
        [MinLength(8)]
        [MaxLength(15)]
        public string Password { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(15)]
        [Compare(nameof(Password))]
        public string RePassword { get; set; }

        

        


    }
}



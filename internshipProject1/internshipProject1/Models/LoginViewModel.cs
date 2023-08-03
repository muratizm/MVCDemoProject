using System.ComponentModel.DataAnnotations;

namespace internshipProject1.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required ! ")]
        [StringLength(10, ErrorMessage = "Username can be max 10 characters !")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required ! ")]
        [MinLength(6, ErrorMessage = "Password can be minimum 6 characters ! ")]
        [MaxLength(15, ErrorMessage = "Password can be maximum 15 characters ! ")]
        public string Password { get; set; }
    }
}



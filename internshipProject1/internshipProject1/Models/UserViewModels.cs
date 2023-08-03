using System.ComponentModel.DataAnnotations;

namespace internshipProject1.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string Username { get; set; }
        public bool Locked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string ProfileImageFileName { get; set; } = "USERICON.png";
        public string Role { get; set; } = "user";
    }
    public class CreateUserModel
    {


        [Required(ErrorMessage = "Username is required!!")]
        [StringLength(15)]
        public string Username { get; set; }



        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        public bool Locked { get; set; }


        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required!!")]
        [MinLength(6)]
        [MaxLength(12)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Re-Password is required!!")]
        [MinLength(6)]
        [MaxLength(12)]
        [Compare(nameof(Password))] 
        public string RePassword { get; set; }



        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "user";

        public string? Done { get; set; }

        
    }

    public class EditUserModel
    {
        [Required(ErrorMessage = "Username is required ! ")]
        [StringLength(10, ErrorMessage = "Username can be max 10 characters !")]
        public string Username { get; set; }

        public string FullName { get; set; }

        public bool Locked { get; set; }

        

        [Required]
        [StringLength(50)]
        public string Role { get; set; } = "user";
    }
}

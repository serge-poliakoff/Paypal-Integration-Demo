using System.ComponentModel.DataAnnotations;

namespace MVCAPPlication.Models
{
    public class LoginUserViewModel
    {
        [Required] [EmailAddress] [Display(Description = "Your email")] public string Email { get; set; }
        [Required][DataType(DataType.Password)] [Display(Description = "Password")] public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}

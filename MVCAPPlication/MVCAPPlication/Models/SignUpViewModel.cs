using System.ComponentModel.DataAnnotations;

namespace MVCAPPlication.Models
{
    public class SignUpViewModel
    {
        [Required][Display(Description = "Full name")] public string Name { get; set; }
        [Required][EmailAddress][Display(Description = "Your email")] public string Email { get; set; }
        [Required][DataType(DataType.Password)][Display(Description = "Password")] public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}

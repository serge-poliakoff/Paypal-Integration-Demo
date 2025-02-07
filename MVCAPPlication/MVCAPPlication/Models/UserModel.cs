using System.ComponentModel.DataAnnotations;

namespace MVCAPPlication.Models
{
    public record class User
    {
        [Key]
        public int Id { get; set; }

        [Required] [MaxLength(50)] public string Name { get; set; }
        [Required] [MaxLength(20)] public string Password { get; set; }
        [Required] [MaxLength(50)] public string Email { get; set; }

        [MaxLength(20)] [Required] public string Role { get; set; } = "User";

    }
}

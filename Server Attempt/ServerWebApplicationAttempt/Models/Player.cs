
using System.ComponentModel.DataAnnotations;

namespace ServerWebApplicationAttempt.Models
{
    public class Player
    {
        [Key] public int Id { get; set; }
        [Required] [MaxLength(40)] public string name { get; set; }
        [Required] [MaxLength(20), MinLength(5)] public string pass { get; set; }

        public ICollection<Side> Sides { get; set; }
        public ICollection<Game> Games { get; set; }
    }
}

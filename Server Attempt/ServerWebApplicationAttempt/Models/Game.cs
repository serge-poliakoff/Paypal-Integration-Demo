using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWebApplicationAttempt.Models
{
    public class Game
    {
        [Key] public int Id { get; set; }
        [MaxLength(8)] public string Status { get; set; } = "waiting"; //waiting -> playing -> finished
        [MaxLength(6)] public string LastMove { get; set; } = "-1x-1b"; // 19x19w 6x17b etc & error for connexion lost

        [InverseProperty(nameof(Side.Game))] public virtual ICollection<Side> Sides { get; set; }
    }
}

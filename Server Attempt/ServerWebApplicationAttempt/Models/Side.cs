
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServerWebApplicationAttempt.Models
{
    public class Side
    {
        [Key] public int Id { get; set; }
        [ForeignKey("Player")] public int PlayerId { get; set; }
        [ForeignKey("Game")] public int GameId { get; set; }

        [Column(TypeName="datetime")] public DateTime Date { get; set; }
        [MaxLength(5)] public string Color { get; set; } = "black";//white or black
        [MaxLength(4)] public string Result { get; set; } = "err";//win, lost, err

        [Required] public virtual Player Player { get; set; }
        [Required] public virtual Game Game { get; set; }
        
        public Side? Enemy { get; set; }
    }
}

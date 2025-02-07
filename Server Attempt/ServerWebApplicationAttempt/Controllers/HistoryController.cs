using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerWebApplicationAttempt.DataAccess;
using ServerWebApplicationAttempt.Models;
using ServerWebApplicationAttempt.TransactionClasses;

namespace ServerWebApplicationAttempt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController : ControllerBase
    {
        [HttpGet("{player_id}")]
        public void GetHistory(int player_id)
        {
            using var context = new DataContext();

            if (context.players.Find(player_id) == null) return;

            Side[] sides = context.sides.Include(s => s.Enemy).Where(s => s.PlayerId == player_id && s.Result != "err").ToArray();

            foreach (Side side in sides)
            {
                try
                {
                    string? enemyName = context.players.Find(side.Enemy.PlayerId)?.name;
                    Console.WriteLine($"{side.Color}\t{side.Result}\tvs {enemyName}\t{side.Date.Date}");
                }catch (NullReferenceException e)
                {
                    Console.WriteLine($"Exception {e} on {side.Id} side");
                }
            }
        }
    }
}

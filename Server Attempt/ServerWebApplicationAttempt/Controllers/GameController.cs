using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerWebApplicationAttempt.DataAccess;
using ServerWebApplicationAttempt.Models;
using ServerWebApplicationAttempt.TransactionClasses;
using System.Drawing;
using System.Text.RegularExpressions;

namespace ServerWebApplicationAttempt.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GamesController : ControllerBase
    {
        private double connexion_timeout = 125;

        [HttpGet("{id}/{side_id}")]
        public string GetLastMove(int id, int side_id)
        {
            using DataContext context = new DataContext();
            Game game = context.games.Include(g => g.Sides).Where(g => g.Id == id).FirstOrDefault();
            if (game == null) return "No game found";

            //checking on connection maintenance
            Side that = context.sides.Find(side_id);
            if (that == null)
            {
                Console.WriteLine("Player not found....");
                return " ";
            }
            that.Date = DateTime.UtcNow;
            context.Update(that);
            context.SaveChanges();

            Side other = that.Enemy;
            if(other != null)
            {
                DateTime other_last_conn = other.Date;
                TimeSpan diff = DateTime.UtcNow - other_last_conn;
                //Console.WriteLine(that.Color + " last connexion: " + that.Date + " (now)");
               // Console.WriteLine(other.Color + " last connexion: " + other_last_conn);
              //  Console.WriteLine("Time difference in seconds: " + diff);
                if(diff.TotalSeconds > connexion_timeout)
                {
                    //opponent lost connexion
                    Console.WriteLine("Considering that " + other.Color + "had lost connexion");
                    game.LastMove = "err";
                    EndGame(game, 'r');
                    context.Update(game);
                    context.SaveChanges();
                }
            }

            return game.LastMove;
        }

        [HttpPost("{game_id}")]
        public string MakeAMove([FromRoute] int game_id, [FromBody] GoMove next_move)
        {
            using DataContext context = new DataContext();
            //checking if the game was good found by a right player
            Game g = context.games.Include(g => g.Sides).Where(g => g.Id == game_id).FirstOrDefault();
            if (g == null) return "game not found";
            if (g.Status != "playing") return "you aren't allowed to make moves here";
            Side side = context.sides.Find(next_move.SideId);
            if (side == null) return "you aren't allowed to make moves here";
            if(side.GameId !=  game_id) return "you aren't allowed to make moves here";

            //checking the move validity
            string move = next_move.move;

            const string incorrect_move_message = "You can't make this move. Check the rules!";

            if (move[move.Length - 1] == g.LastMove[g.LastMove.Length - 1]) return incorrect_move_message;
            if (move[move.Length - 1] != side.Color[0]) return incorrect_move_message;

            if (move == "lostw" || move == "lostb")
            {
                EndGame(g, move[move.Length - 1]);
            }
            else if (move != "passw" && move != "passb")
            {
                Regex reg = new Regex("\\A\\d{1,2}x\\d{1,2}[wb]");
                if (!reg.IsMatch(move)) return incorrect_move_message;

                try
                {
                    string[] info = move.Substring(0, move.Length - 1).Split('x');
                    int m = int.Parse(info[0]);
                    if (m > 18) return incorrect_move_message;
                    m = int.Parse(info[1]);
                    if (m > 18) return incorrect_move_message;
                }
                catch (Exception e)
                {
                    return incorrect_move_message;
                }
            }


            g.LastMove = move;
            context.Update(g);
            context.SaveChanges();

            return "done";
        }

        private void EndGame(Game g, char lost_word)  //lost_word - "w" or "b" in case of finished game, "r" in case of lost connexion
        {
            g.Status = "finished";
            if (lost_word != 'r')
            {
                g.Sides.ElementAt(0).Result = lost_word == 'b' ? "lost" : "won";
                g.Sides.ElementAt(1).Result = lost_word == 'w' ? "lost" : "won";
            }
            else
            {
                g.Sides.ElementAt(0).Result = g.Sides.ElementAt(1).Result = "err";
            }
        }

        [HttpPost("new")]
        public string NewGame(PlayerInfoSec p)  //returns sideId than gameId separated by space
        {
            using DataContext context = new DataContext();
            Console.WriteLine("Tried to start a new game: " + p.name);
            /*
                PlayerInfoSec is used in this case for a security reasons,
                Demanding to send id, we verify that a request was indeed
                from a unity client, as he will get this Id after have logged in.
              (By the way, even in that case, with a knowledge of server's endpoints
               it will be easy to get a game ran without a client...
               maybe some hardcoded encrypter would do the thing...
               but later...)
            */
            Player? playa = context.players.Find(p.Id);
            if (playa == null) return "error"; //invalid player
            if(playa.name != p.name) return "error"; //invalid name
            if(playa.pass != p.password) return "error"; //invalid password

            Game? game = context.games.Include(g => g.Sides).Where(g => g.Status == "playing"
                && g.Sides.Any(s => s.PlayerId == p.Id)).FirstOrDefault();
            if( game != null)       //which means that a player have dropped out of game and will be returned there
            {
                Side side = game.Sides.Where(s => s.PlayerId == p.Id).First();
                return side.Id + " " + game.Id + " " + side.Color;
            }

            Side player_side = new Side()
            {
                PlayerId = playa.Id
            };
            
            game = context.games.Include(g => g.Sides).Where(g => g.Status == "waiting").FirstOrDefault();
            if(game != null)
            {
                if (game.Sides.ElementAt(0).PlayerId == player_side.PlayerId) return "error"; //something strange...
                //by the way, why isn't it possible for player to play with himself? Must consider this moment later...
                player_side.GameId = game.Id;
                player_side.Color = "white";
                player_side.Date = DateTime.UtcNow;
                context.sides.Add(player_side);
                game.Status = "playing";
                game.LastMove = "-1x-1w"; //move for the blacks!
                context.Update(game);
                context.SaveChanges();

                player_side.Enemy = game.Sides.ElementAt(0);
                game.Sides.ElementAt(0).Enemy = player_side;
                context.Update(player_side);
                context.Update(player_side.Enemy);
                context.SaveChanges();


                return player_side.Id + " " + game.Id + " " + player_side.Color;
            }


            game = new Game();
            context.Add(game);
            context.SaveChanges();  //game has now an Id

            player_side.GameId = game.Id;
            player_side.Color = "black";
            player_side.Date = DateTime.UtcNow;
            context.sides.Add(player_side);
            context.SaveChanges();

            Console.WriteLine("Date added to db: " + player_side.Date);


            return player_side.Id + " " + game.Id + " " + player_side.Color;
        }
    }
}

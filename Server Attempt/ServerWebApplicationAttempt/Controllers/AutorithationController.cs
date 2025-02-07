using Microsoft.AspNetCore.Mvc;
using ServerWebApplicationAttempt.DataAccess;
using ServerWebApplicationAttempt.Models;
using ServerWebApplicationAttempt.TransactionClasses;

namespace ServerWebApplicationAttempt.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorisationController : ControllerBase
{
    private string restrictedInName = "@#$%^&*()";

    [HttpPost("registration")]
    public string RegisterOne([FromBody] PlayerInfo newPlayer)
    {
        string name = newPlayer.name;
        string password = newPlayer.password;
        //cheking a validity of given name
        if (name.Length > 40) return "Username must have no more than 40 characters";
        foreach(char c in restrictedInName)
        {
            if(name.Contains(c)) return "You can not put " + c + " character in your username";
        }

        //cheking if a player with given name exists already
        using (var context = new DataContext())
        {

            Player mayExist = context.players.Where(p => p.name == name).FirstOrDefault();
            if (mayExist != null)
                return "Player with this name already exists";
        }

        //cheking a validity of a given password
        if (password.Length < 5) return "Your password must contain at least 5 characters";
        if (password.Length > 10) return "Your password cannot contain more than 10 characters";
        if (password.ToUpper() == password)
           return "You must use at least on lowcase lettre in the password";
        if (password.ToLower() == password)
           return "You must use at least on upcase lettre in your password";
        int temp = 0;
        if (!password.Any(c => Int32.TryParse(c.ToString(), out temp)))
           return "Your password must contain at least one number";
        if (!password.Any(c => restrictedInName.Contains(c)))
           return "Your password must contain one of the followed special symboles: " + restrictedInName;

        //all the check well done, adding to the model
        Player p = new Player()
        {
            name = name,
            pass = password
        };
        using (var context = new DataContext())
        {

            context.Add(p);
            context.SaveChanges();
        }
        
        return p.Id.ToString();
    }
    
    

    [HttpGet("{name}/{password}")]
    public string GetPlayerId([FromRoute] string name, string password)
    {
        using (var context = new DataContext())
        {
            Player? player = context.players.Where(p => p.name == name).FirstOrDefault();
            if (player == null)
                return "username not found";    //wrong name signal
            if (player.pass != password)
                return "wrong password";    //wrong password signal
            return player.Id.ToString();
        }
    }
}

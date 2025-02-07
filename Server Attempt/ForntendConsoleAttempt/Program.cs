using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using ServerWebApplicationAttempt.Models;

namespace FrontendWebApplication
{
    internal class Program
    {
        public static readonly HttpClient HttpClient = new HttpClient()
        {
            BaseAddress = new Uri("http://localhost:5192/")
        };

        public static void RegExpCheck()
        {
            Regex reg = new Regex("\\A\\d{1,2}x\\d{1,2}[wb]");
            Console.WriteLine(reg.IsMatch("12x13w"));
            Console.WriteLine(reg.IsMatch("1x13b"));
            Console.WriteLine(reg.IsMatch("7x8c"));     //false
            Console.WriteLine(reg.IsMatch("x5w"));      //false
            Console.WriteLine(reg.IsMatch("124x13w"));  //false
            Console.WriteLine(reg.IsMatch("1245x13w")); //false
            Console.WriteLine(reg.IsMatch("-2x3w"));    //false
            Console.WriteLine(reg.IsMatch("2x3b"));     //true
        }
        static async Task Main(string[] args)
        {
            /*while (true)
            {
                Console.Write("Choose your username: ");
                string? name = Console.ReadLine();
                if (name == null || name == "") return;
                Console.Write("Then your password ");
                string? password = Console.ReadLine();
                Console.WriteLine("Okay, I'll try to register your account...");
                if(password == null) return;
                Player player = new Player()
                {
                    Id = -1,
                    name = name,
                    pass = password
                };
                using StringContent jsonPlayerInfo = new(
                    JsonSerializer.Serialize(player),
                    Encoding.UTF8,
                    "application/json");
                using HttpResponseMessage response = await HttpClient.PostAsync("Authorisation/registration", jsonPlayerInfo);
                response.EnsureSuccessStatusCode();
                var jsonresp = await response.Content.ReadAsStringAsync();
                Console.WriteLine(jsonresp);
            }*/
            while (true)
            {
                Console.Write("Choose your username: ");
                string? name = Console.ReadLine();
                if (name == null || name == "") return;
                Console.Write("Then your password ");
                string? password = Console.ReadLine();
                Console.WriteLine("Okay, I'll try to log into your account...");
                if (password == null) return;

                string response = await HttpClient.GetStringAsync("Authorisation/" + name + "/" + password);

                Console.WriteLine(response);
            }
        }
    }
}
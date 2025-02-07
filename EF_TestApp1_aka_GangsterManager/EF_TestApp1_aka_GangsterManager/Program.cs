// See https://aka.ms/new-console-template for more information
using EF_TestApp1_aka_GangsterManager.Data;
using EF_TestApp1_aka_GangsterManager.Models;

using System.Net.Http;

//string all_text = new HttpClient().GetStringAsync("https://namescastle.com/gangster-names/").Result;
//Console.WriteLine(all_text.Contains("Al Capone"));

using TestGangstasDatabaseContext context = new TestGangstasDatabaseContext();

var gangs_ordered = context.Gangs.OrderBy(g => g.Id).ToList();



Gang legends = context.Gangs.Where(g => g.GangName == "Legends").FirstOrDefault();
if (legends == null) throw new Exception("No place for the legends");

/*Random rnd = new Random();
int start_pointer = all_text.IndexOf("<span class=\"kt-svg-icon-list-text\">")
    + "<span class=\"kt-svg-icon-list-text\">".Length + 1;
for(int i = 0; i < 10; i++)
{
    int end_pointer = all_text.IndexOf("</span>", start_pointer) - 1;
    string gangsta_name = all_text.Substring(start_pointer, end_pointer - start_pointer);
    start_pointer = all_text.IndexOf("<span class=\"kt-svg-icon-list-text\">", end_pointer)
        + "<span class=\"kt-svg-icon-list-text\">".Length + 1;

    if (gangsta_name.Length > 20) continue; //won't fit in da table

    Gangster ms_g = new Gangster()
    {
        Id = i,
        Name = gangsta_name,
        GangId = legends.Id,
        GunsInDaPants = rnd.Next(10, 100),
        WeedSmoken = rnd.Next(100, 1000),
        Status = "Legend"
    };

    context.Gangsters.Add(ms_g);
    Console.WriteLine(gangsta_name + " is succecfully in a legends gang");
}

context.SaveChanges();*/

var smooth_criminals = context.Gangsters.Where(g => g.GangId == legends.Id).ToList();

foreach (Gangster g in smooth_criminals)
{
    Console.WriteLine($"Id:    {g.Id}");
    Console.WriteLine($"Name:   {g.Name}");
    Console.WriteLine($"Guns in da pants:   {g.GunsInDaPants}");
    Console.WriteLine($"Status:   {g.Status}");
    Console.WriteLine(new string('-', 25));
}
Console.WriteLine("Search ended");

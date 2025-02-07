using Microsoft.AspNetCore.Mvc;
using PaypalExampleApp.DbContexts;
using PaypalExampleApp.Models;
using Newtonsoft.Json;

namespace PaypalExampleApp.Controllers;

public class OrderController : Controller
{
    private IHttpContextAccessor http_acc;
    private PimpDbContext context;

    public OrderController(IHttpContextAccessor http_acc, PimpDbContext context)
    {
        this.http_acc = http_acc;
        this.context = context;
        if (http_acc.HttpContext.Session.GetString("Order") == null)
            http_acc.HttpContext.Session.SetString("Order", 
                JsonConvert.SerializeObject(new List<PuteCommandModel>()));
    }

    [HttpPost]
    public void AddToOrder([FromBody] PuteCommandModel item)
    {
        Console.WriteLine($"Adding to cart {item.Id} for {item.Hours} hours");
        string jsonOrd = check();
        List<PuteCommandModel> order = JsonConvert.DeserializeObject<List<PuteCommandModel>>(jsonOrd)!;
        if (order.Find(puteCommand => puteCommand.Id == item.Id) != null)
        {
            return; //already added, must be something bad here, redirect back
        }
        order.Add(item);
        jsonOrd = JsonConvert.SerializeObject(order);
        http_acc.HttpContext.Session.SetString("Order", jsonOrd);
        return;  //redirect to model's page with view "commanded"
    }

    public string check()
    {
        return http_acc.HttpContext.Session.GetString("Order");
    }

    public IActionResult Index()
    {
        string jsonOrd = check();
        List<PuteCommandModel> order = JsonConvert.DeserializeObject<List<PuteCommandModel>>(jsonOrd)!;

        return View(order);
    }
}

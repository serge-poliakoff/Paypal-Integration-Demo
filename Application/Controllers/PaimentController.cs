using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PayPal.Api;
using PaypalExampleApp.DbContexts;
using PaypalExampleApp.Models;
using System.Text;

namespace PaypalExampleApp.Controllers;

public class PaimentController : Controller
{
    private string ClientId;
    private string ClientSecret;
    private string PaypalUrl;
    private IHttpContextAccessor http_acc;
    private PimpDbContext _context;

    public PaimentController(IConfiguration configuration, IHttpContextAccessor http_acc, PimpDbContext context)
    {
        ClientId = configuration["Paypal:ClientId"]!;
        ClientSecret = configuration["Paypal:Secret"]!;
        PaypalUrl = configuration["Paypal:Url"]!;
        this.http_acc = http_acc;
        _context = context;
    }
    public IActionResult Index()
    {
        ICollection<PuteModel> putes = _context.putes.OrderBy(p => p.Name).ToList();
        return View(putes);
    }

    public IActionResult CreateOrder()
    {
        APIContext context = GetApi();
        string executeUrl = this.Request.Scheme + "://" + this.Request.Host + "/Paiment/Execute";
        Payment payment = InitPayment(context, executeUrl);
        var approveLink = payment.GetHateoasLink("approval_url");

        http_acc.HttpContext.Session.SetString("paymentId", payment.id);

        return Redirect(approveLink.href);
    }

    public IActionResult Execute([FromQuery]bool Cancel = false, string payerId = "")
    {
        if (Cancel)
        {
            return View("Cancel");
        }
        APIContext context = GetApi();
        PaymentExecution paymentExecution = new PaymentExecution() { payer_id = payerId };
        var payment = new Payment()
        {
            id = http_acc.HttpContext.Session.GetString("paymentId")
        };
        var executed  = payment.Execute(context, paymentExecution);
        
        if (executed.state != "approved")
        {
            return View("Cancel");
        }

        return View("Success", executed);
    }

    private Payment InitPayment(APIContext context, string captureUrl)
    {
        var itemList = new ItemList()
        {
            items = new List<Item>()
        };
        string jsonOrd = http_acc.HttpContext.Session.GetString("Order")!;
        List<PuteCommandModel> order = JsonConvert.DeserializeObject<List<PuteCommandModel>>(jsonOrd)!;
        decimal total = 0;
        foreach (var item in order)
        {
            itemList.items.Add(new()
            {
                name = item.Name,
                description = item.Description,
                currency = "USD",
                price = ((int)item.PriceHour).ToString(),
                quantity = ((int)item.Hours).ToString(),
                sku = "asd"
            });
            total += item.Hours * item.PriceHour;
        }

        var payer = new Payer()
        {
            payment_method = "paypal"
        };

        var redirUrls = new RedirectUrls()
        {
            cancel_url = captureUrl + "?Cancel=true",
            return_url = captureUrl
        };
        var amount = new Amount()
        {
            currency = "USD",
            total = ((int)total).ToString()
        };
        var transactionList = new List<Transaction>();

        transactionList.Add(new Transaction()
        {
            description = "What a pay for a good night",
            invoice_number = Guid.NewGuid().ToString(),
            amount = amount,
            item_list = itemList
        });
        Payment payment = new Payment()
        {
            intent = "sale",
            payer = payer,
            transactions = transactionList,
            redirect_urls = redirUrls
        };
        return payment.Create(context);
    }
    
    private APIContext GetApi()
    {
        string accessToken = new OAuthTokenCredential(ClientId, ClientSecret,
            new Dictionary<string, string>() { { "mode", "sandbox" } }).GetAccessToken();
        APIContext context = new APIContext(accessToken);
        context.Config = new Dictionary<string, string>() { { "mode", "sandbox" } };
        return context;
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PaypalExampleApp.DbContexts;
using PaypalExampleApp.Models;

namespace PaypalExampleApp.Controllers
{
    public class HomeController : Controller
    {
        private PimpDbContext _context;

        public HomeController(PimpDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ICollection<PuteModel> putes = _context.putes.OrderBy(p => p.Name).ToList();
            return View(putes);
        }
        [HttpGet("/model/{id}")]
        public IActionResult ViewSlut(int id)
        {
            var slut = _context.putes.Find(id);
            return View("Slut",slut);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

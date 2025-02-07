using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCAPPlication.Models;
using MVCAPPlication.Services;

namespace MVCAPPlication.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }

        public IActionResult SignUpView()
        {
            return View("SignUp");
        }
        public IActionResult SignInView()
        {
            return View("SignIn");
        }
        [Authorize]
        public IActionResult EditView(int Id)
        {
            var model = _context.Users.Find(Id);
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginUserViewModel model){
            if (!ModelState.IsValid)
            {
                return View();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Remind Link", "UserNotFound");
                /*return View("SignUp", new SignUpViewModel()
                {
                    Email = model.Email,
                });*/
                ViewBag.Message = new HtmlString("<br/>Maybe you would like to sign up" +
                    "<a href=\"\\Login\\SignUpView\"> here </a>");
                return View(model);
            } else if (user.Password != model.Password)
            {
                model.Password = string.Empty;
                ModelState.AddModelError("", "Invalid password");
                return View(model);
            }

            var claims = new List<Claim>();
            claims.Add(new(ClaimTypes.Email, user.Email));
            claims.Add(new(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new(ClaimTypes.Name, user.Name));
            claims.Add(new(ClaimTypes.Role, user.Role));
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);


            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties()
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(model.RememberMe?30:1)
                });



            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data");
                return View(model);
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null)
            {
                ModelState.AddModelError("", "User allready exists");
                return RedirectToAction("Index", "Home", model);
            }

            user = new()
            {
                Email = model.Email,
                Password = model.Password,
                Name = model.Name,
                Role = "User"
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            var claims = new List<Claim>();
            claims.Add(new(ClaimTypes.Email, user.Email));
            claims.Add(new(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new(ClaimTypes.Name, user.Name));
            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            Console.WriteLine("Prsistance: " + model.RememberMe);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(new ClaimsPrincipal(identity),
                new AuthenticationProperties()
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(model.RememberMe ? 30 : 1)
                });



            return RedirectToAction(nameof(Index));
        }

        public new async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(User model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Invalid data");
                return View(model);
            }

            _context.Update(model);
            _context.SaveChanges();

            if (HttpContext.User.Claims.Where(claim => claim.Type == ClaimTypes.Role).First().Value != "Admin")
            {
                return await this.SignIn(new LoginUserViewModel()
                {
                    Email = model.Email,
                    Password = model.Password
                });
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value != "Admin")
            {
                return RedirectToAction(nameof(Index));
            }
            
            var user = _context.Users.Find(id);
            if (user == null) { return View("Error"); }
            _context.Users.Remove(user);
            _context.SaveChanges();
            if (user.Email == User.Claims.First(claim => claim.Type == ClaimTypes.Email).Value)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}

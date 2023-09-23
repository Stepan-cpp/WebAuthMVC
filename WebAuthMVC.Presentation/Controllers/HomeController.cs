using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAuth;
using WebAuth.HashProviders;
using WebAuthMVC.Infrastructure.Entities;
using WebAuthMVC.Models;

namespace WebAuthMVC.Controllers;

public class HomeController : Controller
{
   private IPasswordEncryptionService encryptor;

   public User? CurrentUser
   {
      get => (User?) ViewData[nameof(CurrentUser)];
      set => ViewData[nameof(CurrentUser)] = value;
   }

   public HomeController(IPasswordEncryptionService encryptor)
   {
      this.encryptor = encryptor;
   }

   [Authorize]
   public async Task<IActionResult> Account(DbApplicationContext db)
   {
      if (!await CheckAuth(db))
         return Unauthorized();

      return View();
   }

   [HttpPost]
   public async Task<IActionResult> ChangePassword(DbApplicationContext db, PasswordChangeModel pwChange)
   {
      if (!await CheckAuth(db))
         return Unauthorized();

      if (!ModelState.IsValid)
         return View("Account", pwChange);

      CurrentUser!.PasswordHash = encryptor.GetHash(pwChange.NewPassword);
      db.Users.Update(CurrentUser!);
      await db.SaveChangesAsync();
      return Redirect("~/");
   }

   private async Task<bool> CheckAuth(DbApplicationContext db)
   {
      if (User.Identity is {IsAuthenticated: false})
         return false;

      User? user;
      try
      {
         var username = User.Identity.Name;
         var passHash = User.Claims.First().Properties["Password"];

         user = await db.Users.FindAsync(username);
         if (user == null)
         {
            await HttpContext.SignOutAsync();
            CurrentUser = null;
            return false;
         }

         if (!user.VerifyHash(passHash, encryptor))
         {
            await HttpContext.SignOutAsync();
            CurrentUser = null;
            return false;
         }
      }
      catch
      {
         await HttpContext.SignOutAsync();
         CurrentUser = null;
         return false;
      }
      
      CurrentUser = user;

      return true;
   }
   
   public async Task<IActionResult> Index(DbApplicationContext db)
   {
      if (await CheckAuth(db))
      {
         return View(CurrentUser);
      }
      return View();
   }

   [HttpGet]
   public IActionResult Login()
   {
      return View(new LoginModel());
   }

   [HttpGet]
   public async Task<IActionResult> Logout()
   {
      await HttpContext.SignOutAsync();
      return RedirectPermanent("~/");
   }
   
   [AcceptVerbs("GET", "POST")]
   public IActionResult VerifyUsername(string username, DbApplicationContext db)
   {
      if (db.Users.Find(username) is not null)
      {
         return Json($"Username {username} is already in use.");
      }

      return Json(true);
   }
   
   [HttpPost]
   public async Task<IActionResult> Login(string? returnUrl, LoginModel model, DbApplicationContext db)
   {
      User user = db.Users.Find(model.Username);
      if (user is null)
      {
         model.Message = "Invalid username";
         return View(model);
      }

      if (!user.VerifyPassword(model.Password, encryptor))
      {
         model.Message = "Invalid password";
         return View(model);
      }
      
      var claims = new List<Claim> { new (ClaimTypes.Name, user.Username) };
      claims[0].Properties["Password"] = user.PasswordHash;
      
      ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");

      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
      return Redirect(returnUrl ?? "/");
   }

   [HttpGet]
   public IActionResult Register()
   {
      return View(new RegisterModel());
   }
   
   [HttpPost]
   public async Task<IActionResult> Register(RegisterModel model, DbApplicationContext db)
   {
      if (!ModelState.IsValid)
      {
         model.Message = ModelState.Values.First().Errors.First().ErrorMessage;
         return View(model);
      }

      db.Users.Add(new User
      {
         Username = model.Username,
         FirstName = model.FirstName,
         LastName = model.LastName ?? "",
         PasswordHash = AbstractUser.HashPassword(model.Password, encryptor)
      });
      await db.SaveChangesAsync();

      return await Login(null, model, db);
   }
   
   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
   }
}
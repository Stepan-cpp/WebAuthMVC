using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAuth;
using WebAuth.HashProviders;
using WebAuthMVC.Core.Impl;
using WebAuthMVC.Infrastructure.Entities;
using WebAuthMVC.Models;

namespace WebAuthMVC.Controllers;

public class HomeController : Controller
{
   private IPasswordEncryptionService Encryptor { get; }

   public User? CurrentUser
   {
      get => (User?) ViewData[nameof(CurrentUser)];
      set => ViewData[nameof(CurrentUser)] = value;
   }

   public HomeController(IPasswordEncryptionService encryptor)
   {
      Encryptor = encryptor;
   }

   [Authorize]
   public async Task<IActionResult> Account(DbApplicationContext db)
   {
      if (!await CheckAuth(db))
         return Unauthorized();

      return View();
   }

   [HttpPost]
   public async Task<IActionResult> ChangePassword(DbApplicationContext db, PasswordChangeModel passwordModel)
   {
      if (!await CheckAuth(db))
         return Unauthorized();

      if (!ModelState.IsValid)
         return View("Account", passwordModel);

      CurrentUser!.PasswordHash = Encryptor.GetHash(passwordModel.NewPassword);
      db.Users.Update(CurrentUser!);
      await db.SaveChangesAsync();
      
      return Redirect("~/");
   }

   private async void LogOut()
   {
      await HttpContext.SignOutAsync();
      CurrentUser = null;
   }

   private async Task<User?> TryGetUserAsync(IUserCredentials credentials, DbApplicationContext db)
   {
      var user = await db.Users.FindAsync(credentials.Username);
      if (user is null || !user.VerifyHash(credentials.PasswordHash, Encryptor))
         return null;

      return user;
   }

   private async Task<bool> CheckAuth(DbApplicationContext db)
   {
      if (User.Identity is {IsAuthenticated: false})
         return false;

      try
      {
         var username = User.Identity.Name;
         var passHash = User.Claims.First().Properties["Password"];
         User? user;
         if ((user = await TryGetUserAsync(IUserCredentials.FromHash(username, passHash), db)) is not null)
         {
            CurrentUser = user;
            return true;
         }
      }
      catch
      {
         // ignored
      }

      LogOut();
      return false;
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

   private ClaimsPrincipal CreateClaims(IUserCredentials credentials)
   {
      var claims = new List<Claim> { new (ClaimTypes.Name, credentials.Username) };
      claims[0].Properties["Password"] = credentials.PasswordHash;
      return new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies"));
   }
   
   [HttpPost]
   public async Task<IActionResult> Login(string? returnUrl, LoginModel model, DbApplicationContext db)
   {
      User? user = await TryGetUserAsync(IUserCredentials.FromPassword(model.Username, model.Password, Encryptor), db);
      if (user is null)
      {
         model.Message = "Invalid credentials";
         return View(model);
      }

      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, CreateClaims(IUserCredentials.FromUser(user)));
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
         PasswordHash = AbstractUser.HashPassword(model.Password, Encryptor)
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
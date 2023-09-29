using System.Diagnostics;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAuthMVC.BLL.Abstractions;
using WebAuthMVC.BLL.BusinessModels;
using WebAuthMVC.Infrastructure.Entities;
using WebAuthMVC.Models;

namespace WebAuthMVC.Controllers;

public class HomeController : Controller
{
   private IRegistrationService Db { get; }

   public UserModel? CurrentUser
   {
      get => (UserModel?) ViewData[nameof(CurrentUser)];
      set => ViewData[nameof(CurrentUser)] = value;
   }

   public HomeController(IRegistrationService db)
   {
      Db = db;
   }

   [Authorize]
   public async Task<IActionResult> Account()
   {
      if (!await CheckAuth())
         return Unauthorized();

      return View();
   }

   [HttpPost]
   public async Task<IActionResult> ChangePassword(PasswordChangeViewModel passwordViewModel)
   {
      if (!await CheckAuth())
         return Unauthorized();

      if (!ModelState.IsValid)
         return View("Account", passwordViewModel);

      IUserCredentials credentials = IUserCredentials.FromHash(CurrentUser.Username, CurrentUser.PasswordHash);
      Db.ChangeUserPassword(credentials, passwordViewModel.NewPassword);
      
      return Redirect("~/");
   }

   private async void LogOut()
   {
      await HttpContext.SignOutAsync();
      CurrentUser = null;
   }

   private async Task<bool> CheckAuth()
   {
      if (User.Identity is {IsAuthenticated: false})
         return false;

      try
      {
         var username = User.Identity.Name;
         var passHash = User.Claims.First().Properties["Password"];
         IUserCredentials creds = IUserCredentials.FromHash(username, passHash);
         if (Db.Login(creds) is {} user)
         {
            CurrentUser = new UserModel {
               Username = user.Username, 
               FirstName = user.FirstName, 
               IsAdmin = user.IsAdmin, 
               LastName = user.LastName, 
               PasswordHash = user.PasswordHash
            };
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
   
   public async Task<IActionResult> Index()
   {
      if (await CheckAuth())
      {
         return View(CurrentUser);
      }
      return View();
   }

   [HttpGet]
   public IActionResult Login()
   {
      return View(new LoginViewModel());
   }

   [HttpGet]
   public async Task<IActionResult> Logout()
   {
      await HttpContext.SignOutAsync();
      return RedirectPermanent("~/");
   }
   
   [AcceptVerbs("GET", "POST")]
   public IActionResult VerifyUsername(string username)
   {
      if (Db.IsUsernameInUse(username))
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
   public async Task<IActionResult> Login(string? returnUrl, LoginViewModel viewModel)
   {
      var user = Db.Login(new LoginModelDTO {Username = viewModel.Username, Password = viewModel.Password});
      if (user is null)
      {
         viewModel.Message = "Invalid credentials";
         return View("Login", viewModel);
      }

      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, 
         CreateClaims(IUserCredentials.FromHash(user.Username, user.PasswordHash)));
      return Redirect(returnUrl ?? "/");
   }

   [HttpGet]
   public IActionResult Register()
   {
      return View(new RegisterViewModel());
   }
   
   [HttpPost]
   public async Task<IActionResult> Register(RegisterViewModel viewModel)
   {
      if (!ModelState.IsValid)
      {
         viewModel.Message = ModelState.Values.First().Errors.First().ErrorMessage;
         return View(viewModel);
      }

      Db.RegisterUser(new RegisterModelDTO
      {
         Username = viewModel.Username,
         FirstName = viewModel.FirstName,
         LastName = viewModel.LastName ?? "",
         Password = viewModel.Password
      });

      return await Login(null, new LoginViewModel { Username = viewModel.Username, Password = viewModel.Password});
   }
   
   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
   public IActionResult Error()
   {
      return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
   }
}
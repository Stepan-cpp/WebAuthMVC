using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAuthMVC.BLL.Abstractions;
using WebAuthMVC.BLL.BusinessModels;
using WebAuthMVC.Models;

namespace WebAuthMVC.Controllers;

public class HomeController : Controller
{
   private readonly IRegistrationService registrationService;

   public UserModel? CurrentUser
   {
      get => (UserModel?) ViewData[nameof(CurrentUser)];
      set => ViewData[nameof(CurrentUser)] = value;
   }

   public HomeController(IRegistrationService registrationService)
   {
      this.registrationService = registrationService;
   }

   [Authorize]
   public IActionResult Account()
   {
      return View();
   }

   [HttpPost]
   [Authorize]
   public IActionResult ChangePassword(PasswordChangeViewModel passwordViewModel)
   {
      if (!ModelState.IsValid)
         return View("Account", passwordViewModel);

      if (CurrentUser is null)
         return View("Login", new LoginViewModel());
      
      IUserCredentials credentials = IUserCredentials.FromHash(CurrentUser.Username, CurrentUser.PasswordHash);
      registrationService.ChangeUserPassword(credentials, passwordViewModel.NewPassword);
      
      return Redirect("~/");
   }

   public IActionResult Index()
   {
      return View(CurrentUser);
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
      CurrentUser = null;
      return RedirectPermanent("~/");
   }
   
   [AcceptVerbs("GET", "POST")]
   public IActionResult VerifyUsername(string username)
   {
      if (registrationService.IsUsernameInUse(username))
      {
         return Json($"Username {username} is already in use.");
      }

      return Json(true);
   }

   private ClaimsPrincipal CreateClaims(IUserCredentials credentials)
   {
      var claims = new List<Claim> { new (ClaimTypes.Name, credentials.Username) };
      return new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies"));
   }
   
   [HttpPost]
   public async Task<IActionResult> Login(string? returnUrl, LoginViewModel viewModel)
   {
      var user = registrationService.Login(new LoginModelDto {Username = viewModel.Username, Password = viewModel.Password});
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

      registrationService.RegisterUser(new RegisterModelDto
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
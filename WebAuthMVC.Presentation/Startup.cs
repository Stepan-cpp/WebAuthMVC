using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Rewrite;
using WebAuthMVC.BLL.Abstractions;
using WebAuthMVC.BLL.Services;
using WebAuthMVC.DAL.Abstractions;
using WebAuthMVC.DAL.Contexts;
using WebAuthMVC.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IUserVerificationService, BcryptUserVerificationService>();
builder.Services.AddSingleton<ApplicationContext, ApplicationContext>();

builder.Services.AddSingleton<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddSingleton<IRegistrationService, EfRegistrationService>();

builder.Services.AddEntityFrameworkSqlite();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => options.LoginPath="/Home/Login");
builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
var app = builder.Build();

app.UseStaticFiles();
app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}");

var rewriteOptions = new RewriteOptions()
   .AddRedirect("^$", "/Home/Index");
app.UseRewriter(rewriteOptions);

app.UseAuthentication();
app.UseAuthorization();

app.Run();
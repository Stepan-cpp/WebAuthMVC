using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Rewrite;
using WebAuth;
using WebAuth.HashProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPasswordEncryptionService, BcryptPasswordEncryptionService>();
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
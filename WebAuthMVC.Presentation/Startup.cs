using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Rewrite;
using WebAuthMVC.BLL.Configurations;
using WebAuthMVC.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterBllDependencies();
builder.Services.RegisterDalDependencies();

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
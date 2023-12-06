using AspNetCoreHero.ToastNotification;
using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


// Add services to the container.   
    builder.Services.AddDbContext<ShoeStoreContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ShoeStore"))
    );


builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges:new [] {UnicodeRanges.All }));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/dang-nhap.html";
        options.AccessDeniedPath = "/";
    });
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 10;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.UseAuthorization();
app.UseAuthentication();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
});
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

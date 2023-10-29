using Microsoft.EntityFrameworkCore;
using ShoeStore.Models;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


// Add services to the container.   
    builder.Services.AddDbContext<ShoeStoreContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ShoeStore"))
    );


builder.Services.AddSingleton<HtmlEncoder>(HtmlEncoder.Create(allowedRanges:new [] {UnicodeRanges.All }));


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

app.UseAuthorization();

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

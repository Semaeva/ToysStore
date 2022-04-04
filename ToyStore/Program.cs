using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ToyStore.Models;
using ToyStore.ViewsModel;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.Name = ".Cookie"; 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
string mySqlConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<ApplicationContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

builder.Services.AddIdentity<User, IdentityRole>(
    opts =>
    {
        opts.User.RequireUniqueEmail = true;
        opts.Password.RequiredLength = 5;   // минимальная длина
        opts.Password.RequireNonAlphanumeric = false;   // требуются ли не алфавитно-цифровые символы
        opts.Password.RequireLowercase = false; // требуются ли символы в нижнем регистре
        opts.Password.RequireUppercase = false; // требуются ли символы в верхнем регистре
        opts.Password.RequireDigit = false; // требуются ли цифры
    })
                .AddEntityFrameworkStores<ApplicationContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication()
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
        });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();    
app.UseAuthorization();    
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

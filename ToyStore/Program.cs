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
        opts.Password.RequiredLength = 5;   // ����������� �����
        opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
        opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
        opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
        opts.Password.RequireDigit = false; // ��������� �� �����
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

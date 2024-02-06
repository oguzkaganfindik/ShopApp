using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ShopApp.Business.Managers;
using ShopApp.Business.Services;
using ShopApp.Data.Context;
using ShopApp.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString
    ("DefaultConnection");

builder.Services.AddDbContext<ShopAppContext>(options => options.UseSqlServer (connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/");
    options.LogoutPath = new PathString("/");
    options.AccessDeniedPath = new PathString("/");
    // giri� - ��k�� - eri�im engeli durumlar�nda y�nlendirilecek olan adresler.
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
// Auth i�lemleri yap�yorsam, �stteki 2 sat�r yaz�lmal�. Yoksa hata vermez fakat oturum a�maz, yetkilendirme sorgulayamaz.


// AREA i�in yaz�lan ROUTE her zaman Default'un �zerinde olmal�.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{Controller=Dashboard}/{Action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "Default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
    );

app.Run();

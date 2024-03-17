using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
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
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<IImageProcessingService, ImageProcessingManager>();
builder.Services.AddScoped<IImageService, ImageManager>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = new PathString("/GirisYap");
    options.LogoutPath = new PathString("/CikisYap");
    options.AccessDeniedPath = new PathString("/Errors/Error403");
    // giri� - ��k�� - eri�im engeli durumlar�nda y�nlendirilecek olan adresler.
});

// TODO : AccessDeniedPath sorunu ��z�lecek, 403 sayfas� i�in

var contentrootPath = builder.Environment.ContentRootPath;

var keysDirectory = new DirectoryInfo(Path.Combine(contentrootPath, "App_Data", "Keys"));

builder.Services.AddDataProtection()
    .SetApplicationName("ShopApp")
    .SetDefaultKeyLifetime(new TimeSpan(99999, 0, 0))
    .PersistKeysToFileSystem(keysDirectory);

// App_Data -> Keys -> i�erisindeki xml dosyas�na sahip her proje ayn� �ifreleme/�ifre a�ma y�ntemi kullanaca��ndan, birbirlerinin �ifrelerini a�abilirler.


var app = builder.Build();

//app.UseHttpsRedirection();

app.UseStaticFiles(); // wwwroot i�in

app.UseStatusCodePagesWithRedirects("/Errors/Error{0}");

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

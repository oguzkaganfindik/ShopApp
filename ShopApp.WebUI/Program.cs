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
    // giriþ - çýkýþ - eriþim engeli durumlarýnda yönlendirilecek olan adresler.
});

// TODO : AccessDeniedPath sorunu çözülecek, 403 sayfasý için

var contentrootPath = builder.Environment.ContentRootPath;

var keysDirectory = new DirectoryInfo(Path.Combine(contentrootPath, "App_Data", "Keys"));

builder.Services.AddDataProtection()
    .SetApplicationName("ShopApp")
    .SetDefaultKeyLifetime(new TimeSpan(99999, 0, 0))
    .PersistKeysToFileSystem(keysDirectory);

// App_Data -> Keys -> içerisindeki xml dosyasýna sahip her proje ayný þifreleme/þifre açma yöntemi kullanacaðýndan, birbirlerinin þifrelerini açabilirler.


var app = builder.Build();

//app.UseHttpsRedirection();

app.UseStaticFiles(); // wwwroot için

app.UseStatusCodePagesWithRedirects("/Errors/Error{0}");

app.UseAuthentication();
app.UseAuthorization();
// Auth iþlemleri yapýyorsam, üstteki 2 satýr yazýlmalý. Yoksa hata vermez fakat oturum açmaz, yetkilendirme sorgulayamaz.



// AREA için yazýlan ROUTE her zaman Default'un üzerinde olmalý.
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{Controller=Dashboard}/{Action=Index}/{id?}"
    );

app.MapControllerRoute(
    name: "Default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
    );

app.Run();

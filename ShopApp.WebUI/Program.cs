using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString
    ("DefaultConnection");

builder.Services.AddDbContext<ShopAppContext>(options => options.UseSqlServer (connectionString));




var app = builder.Build();

app.MapControllerRoute(
    name: "Default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
    );

app.Run();

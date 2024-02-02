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


var app = builder.Build();

app.MapControllerRoute(
    name: "Default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}"
    );

app.Run();

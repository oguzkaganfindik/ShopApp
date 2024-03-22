using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Entities;

namespace ShopApp.Data.Context
{
    public class ShopAppContext : DbContext
    {
        private readonly IDataProtector _dataProtector;
        public ShopAppContext(DbContextOptions<ShopAppContext> options, IDataProtectionProvider dataProtectionProvider) : base(options)
        {
            _dataProtector = dataProtectionProvider.CreateProtector("security");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FLUENT API -> C# tarafındaki entitylerin sql tablolarına dönüştürülürken özelliklerine yaptığım müdahaleler (Configurations)

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            // Seed Data -> Veritabanı ilk ayaklandırıldığında gelecek olan veri.            
            // var Password = "123";
            // Password = _dataProtector.Protect(Password);

            modelBuilder.Entity<UserEntity>().HasData(new List<UserEntity>()
            {
                new UserEntity() 
                { 
                    Id = 1,
                    FirstName = "Şebnem",
                    LastName = "Ferah",
                    Email = "admin@test.com",
                    Password = _dataProtector.Protect("123"),
                    UserType = Enums.UserTypeEnum.Admin               
                }
            });


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
    }
}

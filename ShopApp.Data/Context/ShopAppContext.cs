using Microsoft.EntityFrameworkCore;
using ShopApp.Data.Entities;

namespace ShopApp.Data.Context
{
    public class ShopAppContext : DbContext
    {
        public ShopAppContext(DbContextOptions<ShopAppContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // FLUENT API -> C# tarafındaki entitylerin sql tablolarına dönüştürülürken özelliklerine yaptığım müdahaleler (Configurations)

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<CategoryEntity> Categories => Set<CategoryEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
    }
}

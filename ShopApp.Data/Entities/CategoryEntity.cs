﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopApp.Data.Entities
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Relational Property

        public List<ProductEntity> Products { get; set; }
    }

    public class CategoryConfiguration : BaseConfiguration<CategoryEntity>
    {
        public override void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {

            builder.Property(x => x.Name)
                .HasMaxLength(40);

            builder.Property(x => x.Description)
                .IsRequired(false);

            base.Configure(builder);
        }
    }
}

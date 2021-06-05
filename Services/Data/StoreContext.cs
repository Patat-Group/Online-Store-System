using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Services.Data
{
    public class StoreContext : IdentityDbContext<User>
    {
        public StoreContext(DbContextOptions options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<ProductAndSubCategory> productAndSubCategories { get; set; }
        public DbSet<VIPAd> VIPAds { get; set; }
        public DbSet<UserRated> UsersRated { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.UserSourceRate)
                .WithMany(ur => ur.UsersDestinationRating)
                .HasForeignKey(fk => fk.UserSourceRateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.UserDestinationRate)
                .WithMany(ur => ur.UsersSourceRating)
                .HasForeignKey(fk => fk.UserDestinationRateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.UserSourceReport)
                .WithMany(ur => ur.UsersDestinationReport)
                .HasForeignKey(fk => fk.UserSourceReportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.UserDestinationReport)
                .WithMany(ur => ur.UsersSourceReport)
                .HasForeignKey(fk => fk.UserDestinationReportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.User)
                .WithMany(ur => ur.Products)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteProduct>()
                .HasOne(p => p.User)
                .WithMany(ur => ur.FavoriteProducts)
                .HasForeignKey(fk => fk.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteProduct>()
                .HasOne(p => p.Product)
                .WithMany(ur => ur.FavoritedByUsers)
                .HasForeignKey(fk => fk.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Report>().Property(s => s.ReportString).IsUnicode();
            modelBuilder.Entity<User>().Property(f => f.FirstName).IsUnicode();
            modelBuilder.Entity<User>().Property(l => l.LastName).IsUnicode();
            modelBuilder.Entity<User>().Property(d => d.Description).IsUnicode();
            modelBuilder.Entity<User>().Property(a => a.Address).IsUnicode();
            modelBuilder.Entity<Product>().Property(ld => ld.LongDescription).IsUnicode();
            modelBuilder.Entity<Product>().Property(sd => sd.ShortDescription).IsUnicode();
            modelBuilder.Entity<Product>().Property(n => n.Name).IsUnicode();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email);
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);
            modelBuilder.Entity<SubCategory>()
                .HasIndex(c => c.Name);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Services.Data
{
    public class StoreContext : IdentityDbContext<User, Role, string>
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<User> Users { get; set; }
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

            modelBuilder.Entity<Category>()
                .Property(n => n.Name).IsRequired().IsUnicode();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserName);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email);
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name);
            modelBuilder.Entity<SubCategory>()
                .HasIndex(c => c.Name);
        }
    }
}
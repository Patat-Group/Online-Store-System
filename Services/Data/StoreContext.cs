using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Services.Data
{
    public class StoreContext :DbContext
    {
        public StoreContext(DbContextOptions options) :base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<VIPAd> VIPAds { get; set; }
        public DbSet<VIPAdImage> VIPAdImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.UserSetRate)
                .WithMany(ur => ur.UsersGetRating)
                .HasForeignKey(fk => fk.UserSetRateId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.UserGetRate)
                .WithMany(ur => ur.UsersSetRating)
                .HasForeignKey(fk => fk.UserGetRateId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Report>()
                .HasOne(r => r.UserSetReport)
                .WithMany(ur => ur.UsersGetReport)
                .HasForeignKey(fk => fk.UserSetReportId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Report>()
                .HasOne(r => r.UserGetReport)
                .WithMany(ur => ur.UsersSetReport)
                .HasForeignKey(fk => fk.UserGetReportId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name);
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name);
            modelBuilder.Entity<SubCategory>()
                .HasIndex(c => c.Name);
        }
    }
}
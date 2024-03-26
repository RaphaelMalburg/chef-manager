using ChefManager.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChefManager.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Preplist> Preplists { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<StoragePlace> StoragePlaces { get; set; }
        public DbSet<UnitMeasure> UnitMeasures { get; set; }
        public DbSet<PreplistItem> PreplistItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserRole<string>>().HasNoKey();
            modelBuilder.Entity<IdentityUserToken<string>>().HasNoKey();
            modelBuilder.Entity<AppUser>()
           .HasKey(u => u.Id);
            modelBuilder.Entity<AppUser>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd(); // Auto-increment id for users 
            modelBuilder.Entity<AppUser>()
                .Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<AppUser>()
                .Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<AppUser>()
                .Property(u => u.CompanyName)
                .HasMaxLength(50)
                .IsRequired();
            modelBuilder.Entity<AppUser>()
                .Property(u => u.IsActive)
                .IsRequired();
            modelBuilder.Entity<AppUser>()
                .Property(u => u.InternalPassword)
                .HasMaxLength(50)
                .IsRequired();

        }


    }
}

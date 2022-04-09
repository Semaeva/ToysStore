using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ToyStore.Models
{
 
        public class ApplicationContext : IdentityDbContext<User>
    {
        public DbSet<Category> categories { get; set; }
        public DbSet<Toys> toys { get; set; }
        public DbSet<toysusers> toysusers { get; set; }
        
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
            {
         //   Database.EnsureCreated();
            }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<Category>()
        //       .ToTable("categories", t => t.ExcludeFromMigrations()).HasKey(p => p.Id);

        //    modelBuilder.Entity<Toys>()
        //      .ToTable("toys", t => t.ExcludeFromMigrations()).HasKey(p => p.Id);

        //    modelBuilder.Entity<User>()
        //       .ToTable("aspnetusers", t => t.ExcludeFromMigrations()).HasKey(p => p.Id);

        //}

    }
}

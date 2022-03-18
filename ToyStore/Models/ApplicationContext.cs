using Microsoft.EntityFrameworkCore;

namespace ToyStore.Models
{
 
        public class ApplicationContext : DbContext
        {
            public DbSet<Category> categories { get; set; }
        public DbSet<Toys> toys { get; set; }

        //  public DbSet<Song> Songs { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
            {
            }
        

    }
}

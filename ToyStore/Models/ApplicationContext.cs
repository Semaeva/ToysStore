using Microsoft.EntityFrameworkCore;

namespace ToyStore.Models
{
 
        public class ApplicationContext : DbContext
        {
           // public DbSet<> Numbers { get; set; }
            //  public DbSet<Song> Songs { get; set; }
            public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
            {
            }
        

    }
}

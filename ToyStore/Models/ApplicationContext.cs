using Microsoft.EntityFrameworkCore;

namespace ToyStore.Models
{
 
        public class ApplicationContext : DbContext
        {
        public DbSet<Category> categories { get; set; }
        public DbSet<Toys> toys { get; set; }
        public DbSet<userOrder> userOrders { get; set; }
        public DbSet<UserRole> userRoles { get; set; }
        public DbSet<Users> users { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
            {

            }
        

    }
}

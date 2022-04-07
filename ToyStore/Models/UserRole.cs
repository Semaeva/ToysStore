namespace ToyStore.Models
{
    public class UserRole
    {

        public int Id { get; set; }
        public string name { get; set; }

        public ICollection<User> Users { get; set; }
        public UserRole()
        {
            Users = new List<User>();
        }
    }
}

namespace ToyStore.Models
{
    public class UserRole
    {

        public int Id { get; set; }
        public string name { get; set; }

        public ICollection<Users> Users { get; set; }
        public UserRole()
        {
            Users = new List<Users>();
        }
    }
}

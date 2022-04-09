namespace ToyStore.Models
{
    public class Toys
    {
        public int Id { get; set; }
        public string toy_name { get; set; }

        public float price { get; set; }


        public string description { get; set; }
        public int? categoryID { get; set; }

        public Category category { get; set; }

        public List<toysusers> toysusers { get; set; } = new List<toysusers>();
        public List<User> users { get; set; } = new List<User>();


    }
}

namespace ToyStore.Models
{
    public class Toys
    {
        public int Id { get; set; }
        public string toy_name { get; set; }

        public float price { get; set; }
        public float summa { get; set; }


        public string description { get; set; }
        public int? categoryID { get; set; }

        public Category category { get; set; }

        public List<userOrder> userOrders { get; set; } = new List<userOrder>();
        public List<Users> user { get; set; } = new List<Users>();


    }
}

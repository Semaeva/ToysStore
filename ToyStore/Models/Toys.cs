namespace ToyStore.Models
{
    public class Toys
    {
        public int Id { get; set; }
        public string toy_name { get; set; }
        public string description { get; set; }
        public int? categoryID { get; set; }

        public Category category { get; set; }
    }
}

namespace ToyStore.Models
{
    public class userOrder
    {
        public int? id { get; set; }
        public int? quantity { get; set; }
       
        public int? toyID { get; set; }
        public Toys? toy { get; set; }

        public string? userId { get; set; }
        public User? user { get; set; }
        

    }
}

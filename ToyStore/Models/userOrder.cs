namespace ToyStore.Models
{
    public class userOrder
    {
        public int id { get; set; }
        public int quantity { get; set; }
       
        public int? toyID { get; set; }
        public Toys toy { get; set; }

        public int? userId { get; set; }
        public Users user { get; set; }
        

    }
}

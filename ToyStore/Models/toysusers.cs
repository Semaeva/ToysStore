namespace ToyStore.Models
{
    public class toysusers
    {
        public int? id { get; set; }
        public int? quantity { get; set; }
       
        public int? toysID { get; set; }
        public Toys? toys { get; set; }

        public string? usersId { get; set; }
        public User? users { get; set; }

        public string? comment { get; set; } = string.Empty;



    }
}

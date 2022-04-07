using Microsoft.AspNetCore.Identity;

namespace ToyStore.Models
{
    public class User : IdentityUser
    {
        public string? Phone { get; set; }   = String.Empty;
        public string? City { get; set; } = String.Empty;
        public string? Area { get; set; } = String.Empty;
        public string? Street { get; set; } = String.Empty;
        public string House { get; set; } = String.Empty;

        public List<userOrder> userOrders { get; set; } = new List<userOrder>();
        public List<Toys> toy { get; set; } = new List<Toys>();
    }
}

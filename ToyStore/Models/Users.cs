using System.ComponentModel.DataAnnotations;

namespace ToyStore.Models
{
    public class Users
    {

        public int id { get; set; }
        [Display(Name = "Имя пользователя")]
        public string email { get; set; }
        [Display(Name = "Пароль")]
        public string passw { get; set; }


        public ICollection<UserRole> userRoles { get; set; }
        public List<userOrder> userOrders { get; set; } = new List<userOrder>();
        public List<Toys> toy { get; set; } = new List<Toys>();
        //  public ICollection<userOrder> userOrders { get; set; }

        public Users()
        {
           // userOrders= new List<userOrder>();
            userRoles= new List<UserRole>();
        }


    }
}

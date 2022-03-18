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

        public int? rolesID { get; set; }
        public UserRole userRole { get; set; }


        public ICollection<Category> toyCatalogs { get; set; }
        public Users()
        {
            toyCatalogs = new List<Category>();
        }


    }
}

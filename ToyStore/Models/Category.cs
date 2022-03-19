using System.ComponentModel.DataAnnotations;

namespace ToyStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Наименование товара")]
        public string category_name { get; set; }

        public ICollection<Users> Users { get; set; }
        public ICollection<Toys> toys { get; set; }

        public Category()
        {
            Users = new List<Users>();
            toys = new List<Toys>();
        }

    }

  
}

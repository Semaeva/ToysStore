using System.ComponentModel.DataAnnotations;

namespace ToyStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Display(Name = "Наименование товара")]
        public string category_name { get; set; }
        public ICollection<Toys> toys { get; set; }
        public Category()
        {
           toys = new List<Toys>();
        }

    }

  
}

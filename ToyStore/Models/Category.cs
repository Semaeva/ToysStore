using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyStore.Models
{
    [Table("categories")]
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

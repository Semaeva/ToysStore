using ToyStore.Models;

namespace ToyStore.ViewsModel
{
    public class IndexViewModel
    {
        public IEnumerable<Toys> Toys { get; set; }
        public IEnumerable<Category> categories { get; set; }

        //ApplicationContext db;

        //public IndexViewModel(ApplicationContext context)
        //{
        //    db = context;

        //}

        public IndexViewModel Searching(ApplicationContext db)
        {
         var categoriesModel = db.categories
         .Select(c => new Category { Id = c.Id, category_name = c.category_name })
         .ToList();
            categoriesModel.Insert(0, new Category { Id = 0, category_name = "Каталог" });
            IndexViewModel category = new IndexViewModel { categories = categoriesModel };
            return category;
        }
    }

}

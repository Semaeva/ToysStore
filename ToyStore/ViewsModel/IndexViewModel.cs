using ToyStore.Models;

namespace ToyStore.ViewsModel
{
    public class IndexViewModel
    {
        public IEnumerable<Toys> Toys { get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}

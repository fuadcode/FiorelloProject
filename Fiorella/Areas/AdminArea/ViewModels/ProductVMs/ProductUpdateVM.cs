
using Fiorello.Models;

namespace Fiorello.Areas.AdminArea.ViewModels.Product
{
    public class ProductUpdateVM
    {
        public IFormFile[] Photos { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductImage> ProductImages { get; set; }
    }
}

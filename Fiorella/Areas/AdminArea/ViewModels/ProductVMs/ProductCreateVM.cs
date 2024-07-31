namespace Fiorello.Areas.AdminArea.ViewModels.Product
{
    public class ProductCreateVM
    {
        public IFormFile[] Photos { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
    }
}

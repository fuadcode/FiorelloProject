using Fiorello.Models;

namespace Fiorello.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<ProductImage> ProductImages { get; set; }
        public IEnumerable<Expert> Experts { get; set; }
        public SliderContent SliderContent { get; set; }
        public Subscription Subscriptions { get; set; }
    }
}
